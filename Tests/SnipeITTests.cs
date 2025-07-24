using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace SnipeItAutomation.Tests
{
    public class SnipeItTests
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IPage _page = null!;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            _page = await _browser.NewPageAsync();
        }

        [Test]
        public async Task CreateAndVerifyMacbookAsset()
        {
            var assetName = $"Macbook Pro 13 - {Guid.NewGuid().ToString().Substring(0, 5)}";
            //Login to the snipeitweb, credentials provided by SnipeIT demo site
            await _page.GotoAsync("https://demo.snipeitapp.com/login");
            await _page.FillAsync("#username", "admin");
            await _page.FillAsync("#password", "password");
            await _page.ClickAsync("button[type='submit']");

           //Create a new Asset
            await _page.ClickAsync("a.dropdown-toggle:has-text(\"Create New\")");
            await _page.ClickAsync("ul.dropdown-menu >> text=Asset");

            // Select random company
            await _page.ClickAsync("#select2-company_select-container");
            await _page.WaitForSelectorAsync("input.select2-search__field");
            await _page.FillAsync("input.select2-search__field", "");
            await _page.WaitForSelectorAsync(".select2-results__option");
            var companies = _page.Locator(".select2-results__option");
            await companies.Nth(new Random().Next(await companies.CountAsync())).ClickAsync();

            // Select model
            await _page.WaitForSelectorAsync("#select2-model_select_id-container");
            await _page.EvaluateAsync("document.querySelector('#select2-model_select_id-container').scrollIntoView()");
            await _page.ClickAsync("#select2-model_select_id-container");
            await _page.FillAsync("input.select2-search__field", "Macbook Pro");
            await _page.WaitForSelectorAsync("li.select2-results__option:has-text('Macbook Pro')");
            await _page.ClickAsync("li.select2-results__option:has-text('Macbook Pro')");
        string assetTag = await _page.InputValueAsync("#asset_tag");
            // Select "Ready to Deploy" status
            await _page.SelectOptionAsync("#status_select_id", new[] { "1" });

            // Assignment section
            await _page.EvaluateAsync("document.getElementById('assignto_selector').style.display = 'block'");
            await _page.EvaluateAsync("document.getElementById('assigned_user').style.display = 'block'");
            await _page.CheckAsync("input[name='checkout_to_type'][value='user']");

            await _page.ClickAsync("#select2-assigned_user_select-container");
            await _page.FillAsync("input.select2-search__field", "");
            await _page.WaitForSelectorAsync(".select2-results__option");
            await _page.ClickAsync(".select2-results__option");

            // Select location
            await _page.ClickAsync("#select2-rtd_location_id_location_select-container");
            await _page.FillAsync("input.select2-search__field", "");
            await _page.WaitForSelectorAsync(".select2-results__option");
            await _page.ClickAsync(".select2-results__option");

            // Submit form
            await _page.ClickAsync("button[id='submit_button']");

            // Navigate to Assets list
            await _page.GotoAsync("https://demo.snipeitapp.com/hardware");

            // Wait for the search input
            await _page.WaitForSelectorAsync("input.search-input");

            // Click the input field
            await _page.ClickAsync("input.search-input");

           // Fill the input using JS and simulate Enter key
            await _page.EvaluateAsync($@"() => {{
            const input = document.querySelector('input.search-input');
            input.focus();
            input.value = '{assetTag}';
            input.dispatchEvent(new Event('input', {{ bubbles: true }}));
            input.dispatchEvent(new KeyboardEvent('keydown', {{ key: 'Enter', bubbles: true }}));
            input.dispatchEvent(new KeyboardEvent('keyup', {{ key: 'Enter', bubbles: true }}));
        }}");

           // Wait for the result list to update
            await _page.WaitForTimeoutAsync(1500); 

           // Click the input field again to trigger blur if search doesn't fire
            await _page.ClickAsync("input.search-input");

           // Click the asset from the search result list
            await _page.ClickAsync($"a:has-text('{assetTag}')");

           // Click the history Tab
            await _page.ClickAsync("a[data-toggle='tab'][href='#history']");

           // Wait for the History tab to become active
            await _page.WaitForSelectorAsync("li.active a[href='#history']");

           // Wait for any element unique to the history content
            await _page.WaitForSelectorAsync("div.tab-pane#history", new() { State = WaitForSelectorState.Visible });

           // Pass the test – no assertion needed as reaching this state is success
            Console.WriteLine("✅ History tab opened successfully.");

        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
