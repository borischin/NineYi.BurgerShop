using NineYi.BurgerShop.Breads;
using NineYi.BurgerShop.Meats;
using NineYi.BurgerShop.Veggies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineYi.BurgerShop.Burgers
{
    public abstract class Burger
    {
        public string Name { get; set; }
        public Bread Bread { get; set; }
        public Veggie Veggie { get; set; }
        public Meat Meat { get; set; }

        /// <summary>
        /// 烹飪漢堡
        /// </summary>
        public void Cook()
        {
            Console.WriteLine("Cooking {0}... \n\tBread used:{1}\n\tVeggie used:{2}\n\tMeat used:{3}", this.Name, this.Bread, this.Veggie, this.Meat);

            Console.WriteLine("Your {0} is ready. Enjoy it!", this.Name);
        }
    }
}
