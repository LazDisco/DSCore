﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSCore.Api;
using DSCore.Ini;
using Microsoft.AspNetCore.Mvc;
using DSCore.Models;
using DSCore.Utilities;
using Newtonsoft.Json;

namespace DSCore.Controllers
{
    [Route("[controller]")]
    public class WeaponController : InheritController
    {
        public IActionResult Index()
        {

            List<Weapon> weapons = Utils.GetAPIResponse<Weapon[]>(Utils.Endpoints.weapon.ToString()).ToList();
            Good[] goods = Utils.GetAPIResponse<Good[]>(Utils.Endpoints.good.ToString());
            Infocard[] infocards = Utils.GetAPIResponse<Infocard[]>(Utils.Endpoints.infocard.ToString());
            Dictionary<uint, string> infocardPairs = new Dictionary<uint, string>();

            for (var index = 0; index < weapons.Count; index++)
            {
                Weapon i = weapons[index];
                Good good = Array.Find(goods, g => g.Equipment == i.Nickname);
                if (good == null)
                {
                    weapons.RemoveAt(index);
                    index--;
                    continue;
                }
                i.Price = good.Price;
                i.BadBuyPrice = good.BadSellPrice;
                i.Combinable = good.Combinable;
                i.GoodSellPrice = good.GoodSellPrice;
                i.BadSellPrice = good.BadSellPrice;
                i.GoodBuyPrice = good.GoodBuyPrice;
                Infocard info = Array.Find(infocards, a => a.Key == i.Name);
                if (info == null)
                    continue;
                infocardPairs[info.Key] = info.Value;
                weapons[index] = i;
            }

            ViewBag.Infocards = infocardPairs;
            return View(weapons);

        }

        [HttpGet("{nickname}")]
        public IActionResult Individual(string nickname)
        {
            Weapon weapon = Utils.GetAPIResponse<Weapon>(Utils.Endpoints.weapon + "/" + nickname);
            Good good = Utils.GetAPIResponse<Good>(Utils.Endpoints.good + "/" + nickname);
            string i = Utils.GetAPIResponse<Infocard>(Utils.Endpoints.infocard + "/" + weapon.Name).Value;
            string ii = Utils.GetAPIResponse<Infocard>(Utils.Endpoints.infocard + "/" + weapon.Infocard).Value;
            ii = Utils.XmlToHtml(ii);
            KeyValuePair<string, string> infocard = new KeyValuePair<string, string>(i, ii);

            weapon.Price = good.Price;
            weapon.BadBuyPrice = good.BadSellPrice;
            weapon.Combinable = good.Combinable;
            weapon.GoodSellPrice = good.GoodSellPrice;
            weapon.BadSellPrice = good.BadSellPrice;
            weapon.GoodBuyPrice = good.GoodBuyPrice;

            ViewBag.Infocard = infocard;
            return View("Individual", weapon);
        }
    }
}
