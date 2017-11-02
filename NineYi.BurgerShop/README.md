## 概念
+ 以簡單工廠為出發點(SimpleBurgerFactory)
+ 將腦人的if/else判斷用dictionary來取代，兼具擴充性又符合開放封閉原則
+ 加上簡單的驗證及例外處理

## 擴充 - 加上Tokyo shop
1. EnumShop 加上 Tokyo
2. 加上新的原料(如果有的話)Bread/Meat/Veggie
3. 加上新的Burger
4. 呼叫SimpleBurgerFactory.AddRecipe建立新的Recipe
5. 完成

