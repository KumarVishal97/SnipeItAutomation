# SnipeItAutomation

This project contains automated UI tests for the Snipe-IT asset management demo site using **Microsoft Playwright** and **NUnit**.

## Project Overview

The test script:
- Logs into the Snipe-IT demo site
- Creates a new Macbook Pro 13" asset with status "Ready to Deploy"
- Assigns the asset to a random user and location
- Verifies the asset creation by searching for it in the assets list
- Navigates to the asset's History tab and validates the page loads

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed
- [Playwright CLI](https://playwright.dev/dotnet/docs/intro) installed and browsers downloaded

```bash
dotnet tool install --global Microsoft.Playwright.CLI
playwright install

How to run tests
Clone the repository:
git clone https://github.com/yourusername/SnipeItAutomation.git
cd SnipeItAutomation

Restore dependencies:
dotnet restore

Run tests:
dotnet test
Tests will launch a Chromium browser and perform the automated steps.

Notes
Tests run in headed mode (Headless = false) so you can watch the browser automation.

Credentials for the demo site are hardcoded as:
Username: admin
Password: password