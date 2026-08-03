Feature: Language Management
    As a Mars user,
    I want to manage languages in my profile,
    So that I can show the languages I know.

Background:
    Given I am logged in to Mars

Scenario Outline: Add language with valid details
    When I add the language "<Language>" with level "<Level>"
    Then the language "<Language>" should be displayed with level "<Level>"

Examples:
    | Language    | Level  |
    | AutoEnglish | Fluent |

Scenario: Delete an existing language
    Given a language exists in the language list
    When I delete the language
    Then the language should be removed from the language list

Scenario: Edit an existing language
    Given a language exists in the language list
    When I edit the language with new valid details
    Then the updated language should be displayed in the language list