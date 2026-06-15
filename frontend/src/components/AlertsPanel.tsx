import type { NotificationMessage } from "../api/types";

type AlertsPanelProps = {
  notifications: NotificationMessage[];
};

export function AlertsPanel({ notifications }: AlertsPanelProps) {
  return (
    <section className="panel">
      <div className="panel-heading">
        <div>
          <h2>Notifications</h2>
          <p>Queue consumers would create these from integration events.</p>
        </div>
      </div>

      <div className="alert-list">
        {notifications.map(message => (
          <article key={message.id} className="alert-item">
            <span className={`severity ${message.severity.toLowerCase()}`}>{message.severity}</span>
            <div>
              <h3>{message.title}</h3>
              <p>{message.body}</p>
            </div>
          </article>
        ))}
      </div>
    </section>
  );
}
