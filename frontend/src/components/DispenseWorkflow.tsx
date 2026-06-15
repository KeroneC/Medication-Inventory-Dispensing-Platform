import { FormEvent, useMemo, useState } from "react";
import type { DispenseResult, InventoryItem } from "../api/types";
import { submitDispense } from "../api/platformApi";

type DispenseWorkflowProps = {
  inventory: InventoryItem[];
  selectedItem?: InventoryItem;
  onDispensed: (result: DispenseResult) => void;
};

export function DispenseWorkflow({ inventory, selectedItem, onDispensed }: DispenseWorkflowProps) {
  const [quantity, setQuantity] = useState(1);
  const [patientId, setPatientId] = useState("PT-1042");
  const [requestedBy, setRequestedBy] = useState("nurse.jordan");
  const [careArea, setCareArea] = useState("Emergency Department");
  const [status, setStatus] = useState("Ready");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const item = useMemo(() => selectedItem ?? inventory[0], [inventory, selectedItem]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!item) {
      setStatus("Select an inventory item before dispensing.");
      return;
    }

    setIsSubmitting(true);
    setStatus("Submitting dispense request...");

    try {
      const result = await submitDispense({
        medicationId: item.medicationId,
        locationId: item.locationId,
        quantity,
        patientId,
        requestedBy,
        careArea
      });

      setStatus(result.message);
      onDispensed(result);
    } catch (error) {
      setStatus(error instanceof Error ? error.message : "Dispense request failed.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <section className="panel workflow-panel">
      <div className="panel-heading">
        <div>
          <h2>Dispense Workflow</h2>
          <p>Submit the path most useful for UI, API, DB, queue, and audit testing.</p>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="dispense-form">
        <label>
          Medication
          <input value={item ? `${item.medicationName} ${item.strength}` : ""} readOnly />
        </label>

        <label>
          Quantity
          <input
            type="number"
            min="1"
            value={quantity}
            onChange={event => setQuantity(Number(event.target.value))}
          />
        </label>

        <label>
          Patient ID
          <input value={patientId} onChange={event => setPatientId(event.target.value)} />
        </label>

        <label>
          Requested by
          <input value={requestedBy} onChange={event => setRequestedBy(event.target.value)} />
        </label>

        <label>
          Care area
          <input value={careArea} onChange={event => setCareArea(event.target.value)} />
        </label>

        <button type="submit" disabled={isSubmitting}>
          {isSubmitting ? "Submitting" : "Dispense"}
        </button>
      </form>

      <div className="workflow-status" role="status">
        {status}
      </div>
    </section>
  );
}
