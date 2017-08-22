using System;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.Finders;
using System.Collections.Generic;
using TestStack.White.UIItems.WindowItems;
using System.Linq;
using NUnit.Framework;
using System.IO;
using TestStack.White.Utility;
using YodaSpeak.Database;

namespace YodaSpeakAutomated
{
    [TestFixture]
    public class YodaSpeakUnitTest
    {
        private TestStack.White.Application application;
        private TestStack.White.UIItems.WindowItems.Window window;
        private TextBox txtOriginalText;
        private Label txtResult;
        Button button;

        [SetUp]
        public void SetUp()
        {
            application = Application.Launch(Path.Combine(TestContext.CurrentContext.TestDirectory, "YodaSpeak.exe"));
            window = application.GetWindow("Yoda Speak");
            txtOriginalText = window.Get<TextBox>(SearchCriteria.ByAutomationId("txtOriginalText"));
            button = window.Get<Button>(SearchCriteria.ByAutomationId("btnTranslate"));
            txtResult = window.Get<Label>(SearchCriteria.ByAutomationId("txtResult"));
        }

        [Test]
        public void Check_TranslateButton_Enable_False()
        {
            txtOriginalText.Text = "";
            Assert.AreEqual(button.Enabled, false);
            txtOriginalText.Enter("Hello");
            Assert.AreEqual(button.Enabled, true);
        }

        [Test]
        public void YodaSpeakTransalate()
        {
            txtOriginalText.Enter("Hello welcome to microsoft team.");
            button.Click();
            window.WaitWhileBusy();
            Assert.IsNotEmpty(txtResult.Text);
            System.Threading.Thread.Sleep(2000);
        }
        [TearDown]
        public void TearDown()
        {
            window.Close();
        }
    }
}
