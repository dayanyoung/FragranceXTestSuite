using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FragranceXTestSuite
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void TestAddToCart()
        {
            string url = "https://www.fragrancex.com/";
            IWebDriver driver = new ChromeDriver();

            string closePopUp = "//*[@id=\"coupon-popup\"]/div/div[1]/img[2]";
            string selectedProduct = "//*[@id=\"content\"]/section/div[1]/div[2]/div[1]/div[2]/div/div/h1/span[1]";
            string addToCartSecondVariant = "/html/body/div[1]/section/div[1]/div[2]/div[1]/div[4]/div/div[4]/div[2]/div/div/div[5]/div[3]/div[1]/form/button";

            driver.Navigate().GoToUrl(url);
            Thread.Sleep(3000);

            // Pre req close the pop up
            IWebElement closePopUpElement = driver.FindElement(By.XPath(closePopUp));

            if (closePopUpElement.Displayed)
            {
                closePopUpElement.Click();
            }

            // Stored all the products/perfume names on a list under 'Top Picks for You' 
            var allProducts = new List<string>();
            for (int i = 1; i <= 15; i++)
            {
                allProducts.Add(driver.FindElement(By.XPath($"//*[@id=\"recommended-items\"]/div/div[1]/div/div[{i}]/div/a/div[2]/div[1]")).Text);
            }

            // Click on the 3rd product from 'Top Picks'
            IWebElement product3Element = driver.FindElement(By.XPath("//*[@id=\"recommended-items\"]/div/div[1]/div/div[3]/div/a/div[2]/div[1]"));

            string product3_name = product3Element.Text;
            product3Element.Click();


            // Verify the product name selected was displayed. Use C. here
            IWebElement selectedProductElement = driver.FindElement(By.XPath(selectedProduct));
            string actual3rdProductElementText = selectedProductElement.Text;

            // Assert 3rd product view
            Assert.IsTrue(string.IsNullOrWhiteSpace(allProducts.Find(p => p.Contains(actual3rdProductElementText))));

            // Add the 2nd product variant to the bag (should be identified with the name e.g. eg. 50ml Eau de Toilette Spray)
            IWebElement addToCartElement = driver.FindElement(By.XPath(addToCartSecondVariant));
            addToCartElement.Click();

            // Verify that count 1 is added on the bag icon
            IWebElement cartCountElement = driver.FindElement(By.XPath("//*[@id=\"AjaxTopCart\"]/a/div"));

            // get attribute value of div class = count it shall have count 1
            Assert.AreEqual(cartCountElement.Text, "1");

            // Update the quantity to 5
            SelectElement quantityDropdown = new SelectElement(driver.FindElement(By.ClassName("cart-qty-select")));
            quantityDropdown.SelectByValue("5");
            Thread.Sleep(3000);

            // add validatiton of 5 count here
            Assert.AreEqual(cartCountElement.Text, "5");

            driver.Quit();
        }
    }
}
