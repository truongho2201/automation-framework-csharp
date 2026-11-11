@UI @allure.epic:SeleniumFramework @allure.suite:Smoke @allure.story:ConfiguredWebsite @normal
Feature: Configured website
  As an automation engineer
  I want to verify that the configured target website can be opened
  So that the Selenium setup is validated through a business-readable scenario

  Scenario: User opens the configured website
    Given the browser is started
    When I navigate to the configured target URL
    Then the page title should not be empty
    And the page body should not be empty
