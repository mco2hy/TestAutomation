using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Web;

namespace TestAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ChromeDriver chromeDriver = new ChromeDriver())
            {
                chromeDriver.Navigate().GoToUrl("https://www.gittigidiyor.com");
                WaitForPageLoad(chromeDriver);

                var policyAlertCloseButtonElement = chromeDriver.FindElementByClassName("policy-alert-close");
                policyAlertCloseButtonElement.Click();

                var loginButtonElements = chromeDriver.FindElementsByClassName("profile-name");

                foreach (IWebElement loginButtonElement in loginButtonElements)
                {
                    if (loginButtonElement.Text == "Giriş Yap")
                    {
                        loginButtonElement.Click();
                        break;
                    }
                }

                WaitForPageLoad(chromeDriver);

                var emailTextElement = chromeDriver.FindElementById("L-UserNameField");
                var passwordTextElement = chromeDriver.FindElementById("L-PasswordField");
                var loginFormButtonElement = chromeDriver.FindElementById("gg-login-enter");

                emailTextElement.SendKeys("adsasd");
                passwordTextElement.SendKeys("asdasd");
                loginFormButtonElement.Click();

                WaitForPageLoad(chromeDriver);

                var searchTextElement = chromeDriver.FindElementById("search_word");
                searchTextElement.SendKeys("samsung");

                var searchButtonElement = chromeDriver.FindElementById("header-search-find-link");
                searchButtonElement.Click();

                WaitForPageLoad(chromeDriver);

                var searchKeywordSpanElement = chromeDriver.FindElementByClassName("search-result-keyword");
                var searchResultCountSpanElement = chromeDriver.FindElementByClassName("result-count");

                string searchKeyword = searchKeywordSpanElement.Text;
                string searchResultCount = searchResultCountSpanElement.Text;
                Console.WriteLine("Arama sonucu >>> Keyword: " + searchKeyword + " Kayıt Sayısı: " + searchResultCount);

                var page2Element = chromeDriver.FindElementByXPath("//*[@id=\"best-match-right\"]/div[5]/ul/li[2]/a");

                if (page2Element != null && page2Element.Displayed)
                {
                    Console.WriteLine("2. sayfa var!");
                    page2Element.Click();

                    WaitForPageLoad(chromeDriver);

                    List<string> productIds = new List<string>();

                    for (int i = 0; i < 3; i++)
                    {
                        var productElements = chromeDriver.FindElementsByClassName("catalog-seem-cell");
                        var productElement = productElements[i];
                        productElement.Click();
                        WaitForPageLoad(chromeDriver);

                        IWebElement favHolderElement = chromeDriver.FindElementById("spp-watch-product");
                        IWebElement favoriteButtonElement = favHolderElement.FindElement(By.ClassName("circleBtn"));

                        if (!favoriteButtonElement.GetAttribute("class").Contains("selected"))
                        {
                            favoriteButtonElement.Click();
                        }

                        string productId = chromeDriver.Url.Split('=')[1];
                        productIds.Add(productId);

                        chromeDriver.Navigate().Back();
                        WaitForPageLoad(chromeDriver);
                    }

                    chromeDriver.Navigate().GoToUrl("https://www.gittigidiyor.com/hesabim/izlediklerim");
                    WaitForPageLoad(chromeDriver);

                    foreach (var productId in productIds)
                    {
                        if (chromeDriver.PageSource.Contains(productId))
                        {
                            Console.WriteLine(productId + " ürün favorilerde bulundu.");
                        }
                        else
                        {
                            Console.WriteLine(productId + " ürün favorilerde bulunamadı.");
                        }
                    }

                    IJavaScriptExecutor javaScript = chromeDriver as IJavaScriptExecutor;
                    javaScript.ExecuteScript("$(\".favorite-product-item\").click();");

                    var deleteButtonElement = chromeDriver.FindElementByClassName("robot-delete-all-button");
                    deleteButtonElement.Click();

                    WaitForPageLoad(chromeDriver);

                    chromeDriver.Navigate().GoToUrl("https://www.gittigidiyor.com");
                    WaitForPageLoad(chromeDriver);

                }
                else
                {
                    Console.WriteLine("2. sayfa yok!");
                }

                Console.ReadLine();
            }
        }

        public static void WaitForPageLoad(IWebDriver webDriver)
        {
            TimeSpan timeout = new TimeSpan(0, 0, 60);

            WebDriverWait wait = new WebDriverWait(webDriver, timeout);

            IJavaScriptExecutor javaScript = webDriver as IJavaScriptExecutor;
            if (javaScript == null) throw new ArgumentException("driver", "driver must support javascript.");

            wait.Until(d => 
            {
                try
                {
                    string readyState = javaScript.ExecuteScript("if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException ex)
                {

                    return ex.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException ex)
                {
                    return ex.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
    }
}