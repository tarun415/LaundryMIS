namespace LaudaryMis.Infrastructure
{
    /// <summary>
    /// Moves the "/u/{slot}" prefix off the request path and onto PathBase.
    ///
    /// Controllers, routes and views stay slot-unaware: because the prefix
    /// becomes PathBase, every link built by Url.Action, the asp-* tag helpers
    /// and "~/" paths is emitted with the current slot already on it, so a tab
    /// keeps navigating inside its own session without any route changes.
    /// </summary>
    public class TabSlotMiddleware
    {
        private readonly RequestDelegate _next;

        public TabSlotMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if (TabSlots.TrySplit(path, out var slot, out var rest))
            {
                context.Items[TabSlots.SlotItemKey] = slot;

                context.Request.PathBase =
                    context.Request.PathBase.Add($"/{TabSlots.Prefix}/{slot}");

                context.Request.Path = rest;

                await _next(context);
                return;
            }

            // Shared assets carry no session and are served from the root.
            if (TabSlots.IsSlotless(path))
            {
                await _next(context);
                return;
            }

            // Anything else arrived without a slot (a bookmark, or the site
            // root). Send it into slot 0 so it picks up a session.
            context.Response.Redirect(
                $"/{TabSlots.Prefix}/0{path}{context.Request.QueryString}");
        }
    }
}
