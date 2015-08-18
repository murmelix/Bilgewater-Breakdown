using Lol.Api.Static;
using Lol.Itemsets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lol.Itemsets.Controllers
{
    public class DataController : Controller
    {
        //
        // GET: /Data/
        public ActionResult LoadItemset(string id, string champ, string map)
        {
            Recommended res = null;
            if (id == "recommanded")
            {
                res = ViewModel.Champions[UppercaseFirst(champ)].Recommended.FirstOrDefault(x => x.Map == map && x.Mode != "INTRO");
            }
            else
            {

            }
            return new JsonResult() { Data = res, JsonRequestBehavior= JsonRequestBehavior.AllowGet};
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
	}
}