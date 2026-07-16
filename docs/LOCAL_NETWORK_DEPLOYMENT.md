Local Network Deployment Checklist for DineProX

Overview
- Target environment: local Windows server or LAN-hosted machine.
- Goals: run fully inside local network, no external cloud dependencies, reliable local backups, LAN authentication options, and LAN-accessible POS terminals.

Checklist

1) Network & Host
- Bind API host to LAN IP or 0.0.0.0 and configure `appsettings.json` URLs.
- Configure CORS to allow POS and manager dashboards on local subnets.
- Reserve static IP for server or use local DNS name.
- Open required ports in Windows Firewall (e.g., 80/443, custom ports).

2) Authentication & Security
- Use local JWT issuer or integrate Windows Authentication/Active Directory if available.
- Enforce HTTPS using a local TLS certificate (self-signed or internal CA).
- Ensure AES-encrypted storage of sensitive data (fingerprints, templates).

3) Backup & Storage (replace Google Drive)
- Use a local network share (SMB) or attached disk for backups.
- Create a scheduled Windows Task or Windows Service to run backups daily.
- Backup targets: database dump (Postgres or local DB), master data CSV export, attachments/images, and logs.
- Example target share: \\BACKUP-SERVER\DineProXBackups or `D:\DineProXBackups`.
- Implement rotation and retention (e.g., keep 30 days).

4) Offline POS & Local Sync
- Use a local SQLite or LiteDB instance on POS terminals for offline cache.
- Implement a sync endpoint on the API for batch transactions.
- Define conflict resolution rules (server wins / idempotent receipts).

5) Remove/Disable External Integrations
- Disable Google Drive, Twilio/WhatsApp, and external report sharing until local adapters are implemented.
- Provide pluggable interfaces so integrations can be added later.

6) Reporting & Exports
- Implement local PDF/CSV generation using server libraries; store generated reports on the backup share.

7) Logging & Monitoring
- Local log files rotating daily; store on backup share too.
- Optional: lightweight monitoring (Prometheus node exporter or Windows Perf counters).

8) Deployment & Start-up
- Publish the `DineProX.HttpApi.Host` to the server and configure as a Windows Service (sc.exe or NSSM).
- Add environment-specific `appsettings.Local.json` with local connection strings and paths.

9) Documentation & SRS Alignment
- Update SRS to mark cloud features as optional and document local alternatives.
- Produce an operations README with configuration steps.

Next actions (recommended)
- Confirm backup target path (SMB share or local disk).
- Implement backup service replacing Google Drive integration.
- Update `appsettings.*.json` and deployment scripts for LAN.

Notes
- This checklist assumes Windows hosting; adjust for Linux servers if needed.
- I'll implement the backup service next after you confirm the backup target path.
