# Testing Strategy

## Unit tests

Focus unit tests on business rules:

- Reject dispense requests with missing patient IDs.
- Reject quantities less than or equal to zero.
- Reject dispense requests when stock is insufficient.
- Mark a result as low stock when quantity falls below the reorder level.

## API tests

Use API tests for service contracts:

- `GET /health` returns a healthy status.
- `GET /inventory` returns seeded records.
- `POST /dispenses` returns `201 Created` for valid requests.
- `POST /dispenses` returns `400 Bad Request` for invalid patient or quantity data.

## Database tests

Use a disposable SQL Server database or Testcontainers when dependencies are allowed:

- Arrange seed medication and stock rows.
- Act by running a dispense operation.
- Assert inventory quantity decreases and an audit row exists.

## Message queue tests

For RabbitMQ or Azure Service Bus:

- Publish `MedicationDispensedEvent`.
- Assert audit and notification consumers process the event.
- Validate retry behavior for transient failures.
- Validate poison-message handling for malformed payloads.

## Playwright tests

Use Playwright for high-value workflows:

- Load dashboard.
- Select medication.
- Submit dispense form.
- Verify a success or validation message.
- Verify audit timeline updates.
