Feature: Language Management
    As a Mars user,
    I want to manage languages in my profile,
    So that I can show the languages I know.

Background:
    Given I am logged in to Mars

@positive
Scenario Outline: Add language with valid details
    When I add the language "<Language>" with level "<Level>"
    Then the language "<Language>" should be displayed with level "<Level>"

Examples:
    | Language    | Level  |
    | AutoEnglish | Fluent |
    | AutoFrench | Basic |
    | AutoHindi | Conversational |
    | AutoGerman | Fluent |



@positive
Scenario Outline: Delete an existing language
    Given the language "<Language>" with level "<Level>" exists
    When I delete the language "<Language>"
    Then the language "<Language>" should be removed from the language list

Examples:
    | Language    | Level  |
    | AutoFrench | Basic |



@positive
Scenario Outline: Edit an existing language
    Given the language "<CurrentLanguage>" with level "<CurrentLevel>" exists
    When I edit the language "<CurrentLanguage>" to "<NewLanguage>" with level "<NewLevel>"
    Then the language "<NewLanguage>" should be displayed with level "<NewLevel>"

Examples:
    | CurrentLanguage | CurrentLevel | NewLanguage | NewLevel          |
    | AutoSpanish     | Conversational | AutoGerman | Native/Bilingual |



@negative @validinput
Scenario Outline: Add a duplicate language
    Given the language "<Language>" with level "<Level>" exists
    When I try to add the language "<Language>" with level "<Level>" again
    Then only one "<Language>" with level "<Level>" should exist
    And the duplicate language message should be displayed

Examples:
    | Language   | Level   |
    | AutoItalian| Fluent  |



@negative @validinput
Scenario Outline: Add the same language with a different level
    Given the language "<Language>" with level "<CurrentLevel>" exists
    When I try to add the language "<Language>" with level "<NewLevel>" again
    Then only one record for the language "<Language>" should exist
    And the language "<Language>" should be displayed with level "<CurrentLevel>"
    And the duplicated data message should be displayed

Examples:
    | Language     | CurrentLevel | NewLevel |
    | AutoJapanese | Basic        | Conversational |


@negative @validinput
Scenario Outline: Update a language to an existing language
    Given the language "<ExistingLanguage>" with level "<Level>" exists
    And the language "<LanguageToUpdate>" with level "<Level>" exists
    When I update the language "<LanguageToUpdate>" to "<ExistingLanguage>" with level "<Level>"
    Then the language already added message should be displayed
    When I cancel the language edit
    Then the language "<ExistingLanguage>" should be displayed with level "<Level>"
    And the language "<LanguageToUpdate>" should be displayed with level "<Level>"

Examples:
    | ExistingLanguage | LanguageToUpdate | Level          |
    | AutoEnglish      | AutoFrench       | Conversational |



@negative @invalidinput
Scenario Outline: Add a language with an empty language field
    When I try to add a language with an empty language field and level "<Level>"
    Then the language validation message should be displayed
    And no language record should be created

Examples:
    | Level |
    | Basic |



@negative @invalidinput
Scenario Outline: Add a language with an empty level field
    When I try to add the language "<Language>" with an empty level
    Then the language validation message should be displayed
    And no language record should be created

Examples:
    | Language   |
    | AutoFrench |


@destructive
Scenario Outline: Add a language with a very large input
    When I add a language containing "<CharacterCount>" characters with level "<Level>"
    Then the very large language should be displayed with level "<Level>"
    And the application should remain responsive

Examples:
    | CharacterCount | Level |
    | 500            | Basic |