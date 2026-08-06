using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApi.Controllers
{
    /// <summary>
    /// Shared base for authenticated controllers. Exposes the current user's id
    /// straight from the validated JWT claims so endpoints never have to trust
    /// a client-supplied id from the route or body.
    /// </summary>
    public abstract class BaseApiController : ControllerBase
    {
        protected int CurrentUserId
        {
            get
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                            ?? User.FindFirst("sub");

                if (claim == null || !int.TryParse(claim.Value, out var id))
                {
                    throw new UnauthorizedAccessException("No valid user id found in the access token.");
                }

                return id;
            }
        }

        protected bool IsAdmin => User.IsInRole("Admin");
    }
}
