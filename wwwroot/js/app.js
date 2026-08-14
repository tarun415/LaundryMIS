/* =========================================================================
   LAUNDRY MIS — shared client helpers
   Loaded once from _Layout, before any view's @section Scripts block.

   Provides:
     - Sensible global DataTables defaults (responsive, styling, language)
     - LMIS.initTable()   consistent table setup with export buttons
     - LMIS.toast()       non-blocking feedback
     - LMIS.confirm()     promise-based confirmation dialog
     - LMIS.money() / LMIS.num()  display formatting
   ========================================================================= */

window.LMIS = (function () {
    'use strict';

    var hasDT = typeof jQuery !== 'undefined' && jQuery.fn && jQuery.fn.dataTable;

    /* ---------------------------------------------------------------------
       DataTables global defaults
       Every table picks these up, including views that call .DataTable({})
       with their own options — anything they pass simply overrides.
       --------------------------------------------------------------------- */
    if (hasDT) {
        jQuery.extend(true, jQuery.fn.dataTable.defaults, {
            responsive: {
                details: {
                    display: jQuery.fn.dataTable.Responsive.display.childRowImmediate,
                    type: 'inline'
                }
            },
            autoWidth: false,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'All']],
            language: {
                search: '',
                searchPlaceholder: 'Search…',
                lengthMenu: '_MENU_ per page',
                info: 'Showing _START_–_END_ of _TOTAL_',
                infoEmpty: 'No records',
                infoFiltered: '(filtered from _MAX_)',
                zeroRecords: 'No matching records found',
                emptyTable: 'No data available',
                paginate: {
                    first: '<i class="fa-solid fa-angles-left"></i>',
                    previous: '<i class="fa-solid fa-angle-left"></i>',
                    next: '<i class="fa-solid fa-angle-right"></i>',
                    last: '<i class="fa-solid fa-angles-right"></i>'
                }
            }
        });
    }

    /* Standard export button set — replaces the copy pasted into ~20 views. */
    function exportButtons(title) {
        var common = title ? { title: title } : {};
        return [
            jQuery.extend({ extend: 'copy',  className: 'btn buttons-copy',  text: '<i class="fa-solid fa-copy"></i>',       titleAttr: 'Copy' }, common),
            jQuery.extend({ extend: 'excel', className: 'btn buttons-excel', text: '<i class="fa-solid fa-file-excel"></i>', titleAttr: 'Excel' }, common),
            jQuery.extend({ extend: 'pdf',   className: 'btn buttons-pdf',   text: '<i class="fa-solid fa-file-pdf"></i>',   titleAttr: 'PDF', orientation: 'landscape', pageSize: 'A4' }, common),
            jQuery.extend({ extend: 'print', className: 'btn buttons-print', text: '<i class="fa-solid fa-print"></i>',      titleAttr: 'Print' }, common)
        ];
    }

    /* Toolbar layout: length + (buttons, search) on top, info + pagination below.
       Uses Bootstrap grid so it stacks instead of colliding on small screens. */
    var TOOLBAR_DOM =
        "<'row g-2 mb-3 align-items-center'" +
            "<'col-12 col-md-auto'l>" +
            "<'col-12 col-md d-flex justify-content-md-end align-items-center gap-2 flex-wrap'Bf>" +
        ">" +
        "<'row'<'col-12'tr>>" +
        "<'row g-2 mt-2 align-items-center'" +
            "<'col-12 col-md-5'i>" +
            "<'col-12 col-md-7'p>" +
        ">";

    /**
     * Initialise a table with the house style.
     * @param {string|jQuery} selector  table element or selector
     * @param {object} [options]        DataTables options; merged over the defaults
     *                                  Pass `exports: false` to hide export buttons,
     *                                  `exportTitle: 'Name'` to title the export files, or
     *                                  `expandable: true` for tables that manage their own
     *                                  detail rows via row.child().
     */
    function initTable(selector, options) {
        if (!hasDT) return null;

        var $el = jQuery(selector);
        if (!$el.length) return null;

        // Guard against double-initialisation when a view also calls .DataTable()
        if (jQuery.fn.dataTable.isDataTable($el)) {
            return $el.DataTable();
        }

        options = options || {};

        var showExports = options.exports !== false;
        var exportTitle = options.exportTitle;
        var expandable = options.expandable === true;
        delete options.exports;
        delete options.exportTitle;
        delete options.expandable;

        var config = jQuery.extend(true, {
            dom: TOOLBAR_DOM,
            buttons: showExports ? exportButtons(exportTitle) : []
        }, options);

        // Responsive and manual expanders both drive row.child(), so they cannot
        // coexist. Expandable tables keep their detail rows and scroll sideways
        // inside .table-responsive instead of collapsing columns.
        if (expandable) {
            config.responsive = false;
        }

        // A table with no export buttons shouldn't reserve the 'B' slot
        if (!showExports && !options.dom) {
            config.dom = TOOLBAR_DOM.replace('Bf', 'f');
        }

        var table = $el.DataTable(config);

        // Re-measure on resize so columns stay sized correctly and Responsive
        // re-picks which columns to collapse (also covers tables revealed later
        // inside tabs or accordions, which measure as zero-width while hidden).
        jQuery(window).on('resize.lmis-' + ($el.attr('id') || 'tbl'), debounce(function () {
            table.columns.adjust();
            if (table.responsive) {
                table.responsive.recalc();
            }
        }, 180));

        return table;
    }

    /**
     * Resolve the DataTables row containing `el`.
     * When Responsive collapses a table, action buttons render inside the
     * detail child row — whose <tr> is not a data row — so fall back to the
     * preceding parent row in that case.
     */
    function rowOf(table, el) {
        var $tr = jQuery(el).closest('tr');
        var row = table.row($tr);
        if (!row.any || !row.any()) {
            row = table.row($tr.prev('tr'));
        }
        return row;
    }

    /** Remove the row containing `el` from `table`, keeping paging position. */
    function removeRowOf(tableSelector, el) {
        if (!hasDT) { jQuery(el).closest('tr').remove(); return; }

        var $t = jQuery(tableSelector);
        if (!jQuery.fn.dataTable.isDataTable($t)) { jQuery(el).closest('tr').remove(); return; }

        var table = $t.DataTable();
        var row = rowOf(table, el);
        if (row.any && row.any()) {
            row.remove().draw(false);
        } else {
            jQuery(el).closest('tr').remove();
        }
    }

    /* ---------------------------------------------------------------------
       Feedback helpers
       --------------------------------------------------------------------- */
    var hasSwal = typeof Swal !== 'undefined';

    function toast(message, type) {
        type = type || 'success';
        if (!hasSwal) { console.log('[' + type + '] ' + message); return; }

        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: type,
            title: message,
            showConfirmButton: false,
            timer: 2600,
            timerProgressBar: true
        });
    }

    function alert(title, message, type) {
        if (!hasSwal) { window.alert(title + '\n\n' + (message || '')); return Promise.resolve(); }
        return Swal.fire({
            icon: type || 'info',
            title: title,
            text: message,
            confirmButtonColor: '#0e7c7b'
        });
    }

    /**
     * Confirmation dialog. Resolves true when the user confirms.
     * Replaces the raw window.confirm() calls scattered through the views.
     */
    function confirm(options) {
        options = options || {};
        var opts = {
            title: options.title || 'Are you sure?',
            text: options.text || '',
            icon: options.icon || 'warning',
            showCancelButton: true,
            confirmButtonText: options.confirmText || 'Yes, continue',
            cancelButtonText: options.cancelText || 'Cancel',
            confirmButtonColor: options.danger === false ? '#0e7c7b' : '#b3261e',
            cancelButtonColor: '#708699',
            reverseButtons: true
        };

        if (!hasSwal) {
            return Promise.resolve(window.confirm(opts.title + '\n\n' + opts.text));
        }
        return Swal.fire(opts).then(function (r) { return !!r.isConfirmed; });
    }

    /* ---------------------------------------------------------------------
       Formatting
       --------------------------------------------------------------------- */
    function num(value, decimals) {
        var n = Number(value);
        if (!isFinite(n)) return '0';
        return n.toLocaleString('en-IN', {
            minimumFractionDigits: decimals || 0,
            maximumFractionDigits: decimals === undefined ? 2 : decimals
        });
    }

    function money(value) {
        return '₹' + num(value, 2);
    }

    /* ---------------------------------------------------------------------
       Misc
       --------------------------------------------------------------------- */
    function debounce(fn, wait) {
        var t;
        return function () {
            var ctx = this, args = arguments;
            clearTimeout(t);
            t = setTimeout(function () { fn.apply(ctx, args); }, wait);
        };
    }

    jQuery(function () {
        // Enable Bootstrap tooltips wherever a view opts in
        if (window.bootstrap && bootstrap.Tooltip) {
            jQuery('[data-bs-toggle="tooltip"]').each(function () { new bootstrap.Tooltip(this); });
        }

        // Auto-dismiss server-rendered flash messages
        jQuery('.alert[data-auto-dismiss]').each(function () {
            var el = this;
            setTimeout(function () { jQuery(el).fadeOut(300); }, 4000);
        });
    });

    return {
        initTable: initTable,
        exportButtons: exportButtons,
        rowOf: rowOf,
        removeRowOf: removeRowOf,
        TOOLBAR_DOM: TOOLBAR_DOM,
        toast: toast,
        alert: alert,
        confirm: confirm,
        num: num,
        money: money,
        debounce: debounce
    };

})();
