/*
 * Phosphorus Five, copyright 2014 - 2017, Thomas Hansen, thomas@gaiasoul.com
 * 
 * This file is part of Phosphorus Five.
 *
 * Phosphorus Five is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License version 3, as published by
 * the Free Software Foundation.
 *
 *
 * Phosphorus Five is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Phosphorus Five.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * If you cannot for some reasons use the GPL license, Phosphorus
 * Five is also commercially available under Quid Pro Quo terms. Check 
 * out our website at http://gaiasoul.com for more details.
 */

using p5.core;
using p5.exp.exceptions;
using p5.auth.helpers;

namespace p5.auth
{
    /// <summary>
    ///     Class wrapping role associated Active Events.
    /// </summary>
    static class Roles
    {
        /// <summary>
        ///     Returns all roles in system.
        /// </summary>
        /// <param name="context">Application Context</param>
        /// <param name="e">Active Event arguments</param>
        [ActiveEvent (Name = "p5.auth.roles.list")]
        public static void p5_auth_roles_list (ApplicationContext context, ActiveEventArgs e)
        {
            if (context.Ticket.Role != "root")
                throw new LambdaSecurityException ("Non-root user tried to list all roles in system", e.Args, context);
            using (new ArgsRemover (e.Args, true)) {
                AuthenticationHelper.GetRoles (context, e.Args);
            }
        }

        /// <summary>
        ///     Returns all access rights in system.
        /// </summary>
        /// <param name="context">Application Context</param>
        /// <param name="e">Active Event arguments</param>
        [ActiveEvent (Name = "p5.auth.access.get")]
        public static void p5_auth_access_get (ApplicationContext context, ActiveEventArgs e)
        {
            using (new ArgsRemover (e.Args, true)) {
                AuthenticationHelper.GetAccess (context, e.Args);
            }
        }
    }
}