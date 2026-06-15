import type { InventoryItem } from "../api/types";

type InventoryTableProps = {
  inventory: InventoryItem[];
  selectedMedicationId: string;
  onSelectMedication: (item: InventoryItem) => void;
};

export function InventoryTable({ inventory, selectedMedicationId, onSelectMedication }: InventoryTableProps) {
  return (
    <section className="panel inventory-panel">
      <div className="panel-heading">
        <div>
          <h2>Inventory</h2>
          <p>Review stock by medication, location, lot, and threshold.</p>
        </div>
      </div>

      <div className="table-wrap">
        <table>
          <thead>
            <tr>
              <th>Medication</th>
              <th>Location</th>
              <th>On hand</th>
              <th>Reorder</th>
              <th>Flags</th>
            </tr>
          </thead>
          <tbody>
            {inventory.map(item => (
              <tr
                key={`${item.medicationId}-${item.locationId}`}
                className={item.medicationId === selectedMedicationId ? "selected-row" : undefined}
                onClick={() => onSelectMedication(item)}
              >
                <td>
                  <strong>{item.medicationName}</strong>
                  <span>{item.strength} {item.form} · {item.lotNumber}</span>
                </td>
                <td>{item.locationName}</td>
                <td>{item.quantityOnHand}</td>
                <td>{item.reorderLevel}</td>
                <td>
                  <div className="flag-list">
                    {item.isLowStock && <span className="status-pill danger">Low stock</span>}
                    {item.isControlledSubstance && <span className="status-pill neutral">Controlled</span>}
                    {item.requiresRefrigeration && <span className="status-pill cool">Cold chain</span>}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}
