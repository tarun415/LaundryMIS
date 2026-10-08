/* =========================================================================
   Sticky helpers for long / wide tables

   1. Sticky header   - when a table is scrolled so its header leaves the top of the
      screen, a copy of the header is pinned just under the top bar (sorting still
      works: a click on the copy is passed on to the real header cell).
   2. Sticky scrollbar - a wide table scrolls sideways inside .table-responsive, but
      its scrollbar sits under the last row. While that scrollbar is below the visible
      screen, a copy of it is pinned to the bottom edge of the window.

   Nothing here changes the layout of a table; the copies are fixed-position overlays.
   ========================================================================= */
(function () {
    'use strict';

    var items = [];

    function topEdge() {
        var bar = document.querySelector('.app-topbar');
        return bar ? bar.getBoundingClientRect().bottom : 0;
    }

    function build(table) {
        var box = table.closest('.table-responsive, .table-wrap') || table;
        var it = { table: table, box: box, sig: '' };

        // ---- sticky scrollbar copy (only for scrolling boxes) ----
        var bar = document.createElement('div');
        bar.className = 'sticky-hscroll';
        bar.setAttribute('aria-hidden', 'true');
        var inner = document.createElement('div');
        bar.appendChild(inner);
        document.body.appendChild(bar);
        it.bar = bar;
        it.inner = inner;

        // Each scrollbar copies the other; the difference check stops the echo
        box.addEventListener('scroll', function () {
            if (Math.abs(bar.scrollLeft - box.scrollLeft) > 1) bar.scrollLeft = box.scrollLeft;
            updateOne(it);
        }, { passive: true });
        bar.addEventListener('scroll', function () {
            if (Math.abs(box.scrollLeft - bar.scrollLeft) > 1) box.scrollLeft = bar.scrollLeft;
        }, { passive: true });

        // ---- sticky header copy ----
        var head = document.createElement('div');
        head.className = 'sticky-thead';
        head.setAttribute('aria-hidden', 'true');
        var copy = document.createElement('table');
        head.appendChild(copy);
        document.body.appendChild(head);
        it.head = head;
        it.copy = copy;

        head.addEventListener('click', function (e) {
            var th = e.target.closest('th');
            if (!th || !table.tHead) return;
            var tr = th.parentElement;
            var realRow = table.tHead.rows[tr.rowIndex];
            var real = realRow && realRow.cells[th.cellIndex];
            if (real) real.click();
        });

        // Keep the copies in step when the table is resized (DataTables redraws, window resize)
        if (window.ResizeObserver) {
            var ro = new ResizeObserver(function () { schedule(); });
            ro.observe(box);
            ro.observe(table);
        }

        return it;
    }

    function updateOne(it) {
        var table = it.table, box = it.box;
        var top = topEdge();
        var vh = window.innerHeight || document.documentElement.clientHeight;
        var tr = table.getBoundingClientRect();
        var visible = table.offsetParent !== null && tr.width > 0;

        // ---- scrollbar copy ----
        var style = window.getComputedStyle(box);
        var scrolls = box !== table && (style.overflowX === 'auto' || style.overflowX === 'scroll')
                      && box.scrollWidth > box.clientWidth + 1;
        var br = box.getBoundingClientRect();
        // The real scrollbar is out of sight when the bottom of the table is below the screen
        var needBar = visible && scrolls && br.bottom > vh && br.top < vh - 40;
        if (needBar) {
            it.bar.style.left = br.left + 'px';
            it.bar.style.width = br.width + 'px';
            it.inner.style.width = box.scrollWidth + 'px';
            it.bar.classList.add('show');
            if (Math.abs(it.bar.scrollLeft - box.scrollLeft) > 1) it.bar.scrollLeft = box.scrollLeft;
        } else {
            it.bar.classList.remove('show');
        }

        // ---- header copy ----
        var thead = table.tHead;
        var needHead = false;
        if (visible && thead && thead.offsetParent !== null && !table.closest('.modal, .no-sticky-head')) {
            var hr = thead.getBoundingClientRect();
            // Header has scrolled up under the top bar, and the table still has rows on screen
            needHead = hr.height > 0 && hr.top < top - 1 && tr.bottom > top + hr.height + 24
                       && br.right > 0 && br.left < (window.innerWidth || 0);
            if (needHead) {
                var widths = [];
                var cells = thead.rows[0] ? thead.rows[0].cells : [];
                for (var i = 0; i < cells.length; i++) widths.push(Math.round(cells[i].getBoundingClientRect().width * 10) / 10);
                var sig = thead.innerHTML + '|' + widths.join(',') + '|' + Math.round(tr.width);

                if (sig !== it.sig) {
                    it.sig = sig;
                    it.copy.className = table.className;
                    it.copy.style.cssText = 'table-layout:fixed;margin:0;width:' + tr.width + 'px';
                    var clone = thead.cloneNode(true);
                    clone.querySelectorAll('[id]').forEach(function (n) { n.removeAttribute('id'); });
                    var ccells = clone.rows[0] ? clone.rows[0].cells : [];
                    for (var j = 0; j < ccells.length && j < widths.length; j++) {
                        ccells[j].style.width = ccells[j].style.minWidth = ccells[j].style.maxWidth = widths[j] + 'px';
                    }
                    while (it.copy.firstChild) it.copy.removeChild(it.copy.firstChild);
                    it.copy.appendChild(clone);
                }

                // Clip to the table's box and slide with its horizontal scroll
                var clipLeft = box !== table ? br.left : tr.left;
                var clipWidth = box !== table ? box.clientWidth : tr.width;
                it.head.style.top = top + 'px';
                it.head.style.left = clipLeft + 'px';
                it.head.style.width = clipWidth + 'px';
                it.copy.style.marginLeft = (tr.left - clipLeft) + 'px';
                it.head.classList.add('show');
            }
        }
        if (!needHead) it.head.classList.remove('show');
    }

    function updateAll() { items.forEach(updateOne); }

    function scan() {
        document.querySelectorAll('table').forEach(function (table) {
            if (table.__sticky || !table.tHead) return;
            // Skip the copies we made ourselves and tables nested in another table
            if (table.closest('.sticky-thead') || table.parentElement.closest('table')) return;
            table.__sticky = true;
            items.push(build(table));
        });
        updateAll();
    }

    var queued = false;
    function schedule() {
        if (queued) return;
        queued = true;
        // setTimeout rather than requestAnimationFrame: frames are not produced while the tab is hidden
        window.setTimeout(function () { queued = false; scan(); }, 30);
    }

    document.addEventListener('DOMContentLoaded', function () {
        scan();
        window.addEventListener('scroll', updateAll, { passive: true });
        window.addEventListener('resize', schedule);
        // Tables change when DataTables draws, filters or expands a row
        if (window.MutationObserver) {
            new MutationObserver(schedule).observe(document.querySelector('.app-content') || document.body,
                { childList: true, subtree: true, attributes: true, attributeFilter: ['class', 'style'] });
        }
        window.setTimeout(schedule, 600);
    });
})();
