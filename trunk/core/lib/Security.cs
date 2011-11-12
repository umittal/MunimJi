using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Web;

namespace munimji.core.lib {
    public static class Security {
        public static IIdentity GetMyIdentity() {
            if (ServiceSecurityContext.Current != null) {
                return ServiceSecurityContext.Current.WindowsIdentity;
            }
            if (HttpContext.Current != null) {
                return HttpContext.Current.User.Identity;
            }
            if (Thread.CurrentPrincipal != null) {
                return Thread.CurrentPrincipal.Identity;
            }
            if (WindowsIdentity.GetCurrent() != null) {
                return WindowsIdentity.GetCurrent();
            }
            return WindowsIdentity.GetAnonymous();
        }
    }
}