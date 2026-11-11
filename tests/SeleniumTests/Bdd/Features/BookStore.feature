@UI @allure.epic:DemoQA @allure.suite:BookStore @allure.story:AddBookToCollection @critical
Feature: Add a book to the user's collection
  As a DemoQA user
  I want to add a book from the Book Store
  So that it is available in my collection

  Background:
    Given the user's book collection is empty
    And the user is logged in
    And the user opens the Book Store

  Scenario: User searches for a book and adds it to the collection
    When the user searches for "Git Pocket Guide"
    And the user opens the "Git Pocket Guide" book details
    Then the book details title should be "Git Pocket Guide"
    When the user adds the book to the collection
    Then the add-to-collection confirmation should be displayed
