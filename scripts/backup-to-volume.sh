#!/bin/bash
set -euo pipefail

BACKUP_ROOT="${BACKUP_ROOT:-/var/backups/dineprox}"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
TARGET_DIR="$BACKUP_ROOT/$TIMESTAMP"
mkdir -p "$TARGET_DIR"

echo "Creating backup in $TARGET_DIR"
mkdir -p "$TARGET_DIR/logs"
cp -R ./src/DineProX.HttpApi.Host/Logs "$TARGET_DIR/logs" 2>/dev/null || true
find . -maxdepth 3 -type f \( -name '*.db' -o -name '*.json' -o -name '*.xml' \) | tar -czf "$TARGET_DIR/files.tgz" -T -

echo "Backup complete: $TARGET_DIR"
