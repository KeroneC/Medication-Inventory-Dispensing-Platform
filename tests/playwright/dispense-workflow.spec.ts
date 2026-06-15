import { expect, test } from "@playwright/test";

test("user can review inventory and submit a dispense request", async ({ page }) => {
  await page.goto("/");

  await expect(page.getByRole("heading", { name: "Medication Inventory & Dispensing" })).toBeVisible();
  await expect(page.getByRole("heading", { name: "Inventory" })).toBeVisible();

  await page.getByRole("row", { name: /Morphine Sulfate/i }).click();
  await page.getByLabel("Quantity").fill("1");
  await page.getByLabel("Patient ID").fill("PT-1042");
  await page.getByRole("button", { name: "Dispense" }).click();

  await expect(page.getByRole("status")).not.toContainText("Ready");
});
