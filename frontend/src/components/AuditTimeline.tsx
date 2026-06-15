import type { AuditRecord } from "../api/types";

type AuditTimelineProps = {
  records: AuditRecord[];
};

export function AuditTimeline({ records }: AuditTimelineProps) {
  return (
    <section className="panel">
      <div className="panel-heading">
        <div>
          <h2>Audit Trail</h2>
          <p>Traceability for compliance, troubleshooting, and defect reports.</p>
        </div>
      </div>

      <ol className="timeline">
        {records.map(record => (
          <li key={record.id}>
            <time>{new Date(record.occurredAt).toLocaleString()}</time>
            <strong>{record.action}</strong>
            <span>{record.actor} · {record.entityType}</span>
            <p>{record.summary}</p>
          </li>
        ))}
      </ol>
    </section>
  );
}
