using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialASPNET.Models
{
    public class Subscribe
    {
        private int idSubscriber;
        private int idSubscription;

        public int IdSubscriber { get => idSubscriber; set => idSubscriber = value; }
        public int IdSubscription { get => idSubscription; set => idSubscription = value; }
    }
}
