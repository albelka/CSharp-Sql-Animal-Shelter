using Nancy;
using System.Collections.Generic;
using System;


namespace Shelter
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["TEMPLATE.cshtml"];
      };

    }
  }
}
