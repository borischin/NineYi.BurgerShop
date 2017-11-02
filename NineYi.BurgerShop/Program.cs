using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NineYi.BurgerShop.Burgers;
using NineYi.BurgerShop.Breads;
using NineYi.BurgerShop.Veggies;
using NineYi.BurgerShop.Meats;

namespace NineYi.BurgerShop
{
    enum EnumShop
    {
        Taipei = 1,
        NewYork,
        Tokyo
    }

    enum EnumBurger
    {
        Chicken = 1,
        Pork
    }

    class Program
    {
        static void Main(string[] args)
        {
            //// 0. Prepare Recipes
            PrepareRecipes();

            do
            {
                try
                {
                    //// 1. 使用者點餐
                    Console.Write("Which shop do you like? {0}: ", PrintEnums(GetEnums<EnumShop>()));
                    EnumShop shopChoice = parseInput<EnumShop>(Console.ReadLine());

                    var definedBurgers = SimpleBurgerFactory.GetDefinedBurgers(shopChoice);
                    Console.Write("What burger would you like? {0}: ", PrintEnums(definedBurgers));
                    EnumBurger burgerChoice = parseInput<EnumBurger>(Console.ReadLine(), definedBurgers);

                    //// 2. Create 漢堡
                    var burger = SimpleBurgerFactory.Create(shopChoice, burgerChoice);

                    //// 3. 烹飪漢堡
                    burger.Cook();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (ExitOrGoOn());
        }

        static void PrepareRecipes()
        {
            SimpleBurgerFactory.AddRecipe(EnumShop.Taipei, EnumBurger.Chicken, () =>
                 new TaipeiChickenBurger
                 {
                     Bread = new WhiteBread(),
                     Veggie = new Tomato(),
                     Meat = new TaiwanChicken()
                 }
            );
            SimpleBurgerFactory.AddRecipe(EnumShop.Taipei, EnumBurger.Pork, () =>
                 new TaipeiPorkBurger
                 {
                     Bread = new WhiteBread(),
                     Veggie = new Tomato(),
                     Meat = new Tenderloin()
                 }
            );
            SimpleBurgerFactory.AddRecipe(EnumShop.NewYork, EnumBurger.Chicken, () =>
                 new NewYorkChickenBurger
                 {
                     Bread = new WhiteBread(),
                     Veggie = new Onion(),
                     Meat = new Turkey()
                 }
            );
            SimpleBurgerFactory.AddRecipe(EnumShop.NewYork, EnumBurger.Pork, () =>
                 new NewYorkPorkBurger
                 {
                     Bread = new WhiteBread(),
                     Veggie = new Onion(),
                     Meat = new Bacon()
                 }
            );
            SimpleBurgerFactory.AddRecipe(EnumShop.Tokyo, EnumBurger.Chicken, () =>
                 new TokyoChickenBurger
                 {
                     Bread = new WheatBread(),
                     Veggie = new Tomato(),
                     Meat = new Bacon()
                 }
            );
        }

        static Func<bool> ExitOrGoOn = () =>
        {
            var exitKey = ConsoleKey.Q;
            Console.Write("Press '{0}' to quit or any other key to go on...", exitKey.ToString());
            var goOn = Console.ReadKey().Key != exitKey;
            Console.WriteLine();
            Console.WriteLine();
            return goOn;
        };


        static IEnumerable<T> GetEnums<T>() where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception(string.Format("Invalid enum type({0}).", type.ToString()));
            }

            return Enum.GetValues(typeof(T)).OfType<T>();
        }
        static string PrintEnums<T>(IEnumerable<T> enums) where T : struct, IConvertible
        {
            var enumTexts = enums.Select(x => string.Format("({0}){1}", Convert.ToInt32(x), x));
            return string.Join(" ", enumTexts);
        }

        static T parseInput<T>(string input, IEnumerable<T> acceptableList = null) where T : struct
        {
            T output;
            if (Enum.TryParse(input, out output) == false || Enum.IsDefined(typeof(T), output) == false)
            {
                throw new Exception(string.Format("Invalid {0}({1})", typeof(T).Name, input));
            }

            acceptableList = acceptableList ?? Enumerable.Empty<T>();
            if (acceptableList.Any() && acceptableList.Contains(output) == false)
            {
                throw new Exception(string.Format("Unacceptable {0}({1})", typeof(T).Name, input));
            }

            return output;
        }
    }

    class SimpleBurgerFactory
    {
        private static IDictionary<string, Func<Burger>> _recipes = new Dictionary<string, Func<Burger>>();

        private static Func<EnumShop, EnumBurger, string> GetRecipeKey = (shopChoice, burgerChoice) => string.Format("{0}-{1}", shopChoice, burgerChoice);
        public static void AddRecipe(EnumShop shopChoice, EnumBurger burgerChoice, Func<Burger> maker)
        {
            _recipes.Add(GetRecipeKey(shopChoice, burgerChoice), maker);
        }

        public static IEnumerable<EnumBurger> GetDefinedBurgers(EnumShop shopChoice)
        {
            return _recipes.Where(x => x.Key.StartsWith(string.Format("{0}-", shopChoice)))
                .Select(x => (EnumBurger)Enum.Parse(typeof(EnumBurger), x.Key.Split(new char[] { '-' })[1]));
        }

        public static Burger Create(EnumShop shopChoice, EnumBurger burgerChoice)
        {
            Func<Burger> burgerMaker = null;
            if (!_recipes.TryGetValue(GetRecipeKey(shopChoice, burgerChoice), out burgerMaker))
            {
                throw new Exception(string.Format("Undefined burger recipe: Shope({0}), Burger({1})", shopChoice, burgerChoice));
            }
            return burgerMaker();
        }
    }
}
