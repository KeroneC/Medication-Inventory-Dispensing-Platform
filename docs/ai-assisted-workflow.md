# AI-Assisted Engineering Workflow

Use AI as an accelerator, not as an unchecked author.

## Good uses

- Generate initial test cases from acceptance criteria.
- Convert BDD scenarios into Playwright test skeletons.
- Draft API documentation from endpoint code.
- Summarize defects with reproduction steps, expected result, actual result, and suspected component.
- Review code for missing validation, edge cases, security risks, and performance issues.

## Required human validation

- Run the code and tests.
- Check generated tests against real requirements.
- Review AI-generated code for maintainability and security.
- Confirm healthcare terminology and workflow assumptions with domain experts.

## Prompting pattern

1. Give the AI the exact service, endpoint, or workflow.
2. State the role: test engineer, developer, reviewer, or documentation writer.
3. Provide acceptance criteria and constraints.
4. Ask for edge cases before asking for final artifacts.
5. Validate the output with code, tests, and domain review.
