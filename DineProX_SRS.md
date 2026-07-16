# DineProX – Software Requirements Specification (SRS)

**Prepared by:** Bashanta Raj Dangal  
**Company:** Syronx Technology  
**Date:** 13 November 2025

---

## Version Control

| Version Number | Date | Description of Changes | Author/Approver |
|---|---|---|---|
| 1.0 | 13 November 2025 | Initial draft | Bashanta Raj Dangal |

---

## Table of Contents

1. [Introduction](#10-introduction)
   - [1.1 Purpose](#11-purpose)
   - [1.2 Scope](#12-scope)
   - [1.3 Audience](#13-audience)
2. [Overall Description](#20-overall-description)
   - [2.1 System Type](#21-system-type)
   - [2.2 Architecture](#22-architecture)
   - [2.3 Users & Roles](#23-users--roles)
3. [System Features](#30-system-features)
   - [3.1 POS & Order Management](#31-pos--order-management)
   - [3.2 Inventory Management](#32-inventory-management)
   - [3.3 HRMS – Employee & Attendance](#33-hrms--employee--attendance)
   - [3.4 Financial Reporting](#34-financial-reporting)
   - [3.5 Offline POS Functionality](#35-offline-pos-functionality)
   - [3.6 Security & Access Control](#36-security--access-control)
   - [3.7 Notifications & Messaging](#37-notifications--messaging)
   - [3.8 Data Backup & Cloud Integration](#38-data-backup--cloud-integration)
4. [Non-Functional Requirements](#40-non-functional-requirements)
5. [External Interface Requirements](#50-external-interface-requirements)
6. [Other Requirements](#60-other-requirements)
7. [System Architecture Diagram (Conceptual)](#70-system-architecture-diagram-conceptual)
8. [Server Requirements](#80-server-requirements)
9. [System Setup & Deployment](#90-system-setup--deployment)
10. [Master Data Setup](#100-master-data-setup)

---

## 1.0 Introduction

### 1.1 Purpose

The purpose of this document is to define the functional and non-functional requirements for DineProX, a single-tenant restaurant and café management system. This document will guide the development, QA, deployment, and maintenance of the system.

### 1.2 Scope

DineProX will include the following modules:

- POS (Point of Sale) with online/offline support
- Inventory and Supplier Management
- HRMS (Human Resource Management System) with fingerprint attendance and payroll
- Financial Reporting and Analytics
- Cloud Backup via Google Drive
- Report Sharing via Email and WhatsApp
- Role-Based Access Control (RBAC) and audit logging

### 1.3 Audience

The intended audience for this document includes:

- Developers
- QA engineers
- Project managers
- System administrators
- Stakeholders

---

## 2.0 Overall Description

### 2.1 System Type

The system is a web-based application with offline POS capability.

### 2.2 Architecture

- **Presentation Layer:** Web POS and Manager Dashboards
- **Application Layer:** API server handling business logic
- **Data Layer:** PostgreSQL database, local offline storage, Google Drive backup

### 2.3 Users & Roles

| Role | Access & Capabilities |
|---|---|
| Administrator | Full access to all modules, server settings, backups, and logs |
| Manager | POS, Inventory, HR, Reports, Notifications |
| Cashier | POS only (sales, billing, receipts) |
| Waitstaff | Order taking, table management |
| Chef | Kitchen order display |
| HR Officer | Employee management, attendance, payroll |

---

## 3.0 System Features

### 3.1 POS & Order Management

- Create, modify, and close orders
- Table management for dine-in
- Payment processing (cash, card, wallet)
- Receipt generation (printed or PDF)
- Offline POS support with local storage and auto-sync
- Kitchen display integration

### 3.2 Inventory Management

- Track stock levels per item
- Supplier management
- Auto-adjust inventory when sales occur
- Low-stock alerts and notifications
- Purchase order management

### 3.3 HRMS – Employee & Attendance

- Employee registration with roles and departments
- Fingerprint enrollment for attendance
- Shift management
- Payroll calculation based on attendance and shifts
- Attendance and payroll reports

### 3.4 Financial Reporting

- Daily, weekly, monthly reports
- Income, expenses, profit/loss calculation
- Export to PDF/CSV
- Cloud backup to Google Drive
- Sharing via Email & WhatsApp

### 3.5 Offline POS Functionality

- Continue sales during network outage
- Local storage of transactions
- Offline status indicator
- Auto-sync when online

### 3.6 Security & Access Control

- Role-based access (RBAC)
- Data encryption (AES-256)
- JWT/OAuth2 authentication
- Audit logging of user actions

### 3.7 Notifications & Messaging

- Email and WhatsApp notifications for reports and alerts
- Configurable recipient list
- Notifications for low stock, backup failures, or offline POS

### 3.8 Data Backup & Cloud Integration

- Automated daily backups to Google Drive
- Hourly incremental backups
- Cloud-based report sharing

---

## 4.0 Non-Functional Requirements

- **Performance:** <2s latency for POS transactions, <5s report generation
- **Reliability:** 99.9% uptime
- **Security:** Encrypted storage and transmission, HTTPS/TLS 1.3
- **Scalability:** Modular system, multiple POS terminals supported
- **Maintainability:** Modular code structure for updates and fixes
- **Compliance:** Meets Australian data protection standards

---

## 5.0 External Interface Requirements

### 5.1 User Interface

- Web POS, dashboards, responsive design

### 5.2 Hardware

- POS terminals, fingerprint scanners, printers

### 5.3 Software

- Browser, local database for offline mode

### 5.4 Cloud Integration

- Google Drive API for backup

### 5.5 Messaging

- Twilio/WhatsApp API for report sharing

---

## 6.0 Other Requirements

- Fingerprint templates must be encrypted
- Daily auto-sync of offline POS data
- Reports retention: 1 year
- Attendance retention: 3 years
- Role-based permissions and activity logs

---

## 7.0 System Architecture Diagram (Conceptual)

- **Presentation Layer:** POS terminals, HR dashboard, Manager dashboards
- **Application Layer:** API server with business logic
- **Data Layer:** PostgreSQL database, local offline storage, Google Drive backup
- **Data Flow:** POS → Inventory → HRMS → Reports → Cloud → Email/WhatsApp
- **Offline POS Caches:** Sync when online

---

## 8.0 Server Requirements

### Minimum Requirements

| Component | Minimum Requirement | Notes |
|---|---|---|
| CPU | 4 vCPUs | Sufficient for small-scale single-tenant operations |
| RAM | 8 GB | Can support POS, HRMS, and reports with moderate load |
| Storage | 100 GB SSD | Enough for database, offline POS cache, and logs |
| Operating System | Ubuntu Server 22.04 LTS | Stable, long-term support |
| Web Server | Nginx / Apache | To host backend API and dashboards |
| Database | PostgreSQL 16+ | Stores all application and POS data |
| Backend Runtime | Node.js 20+ or Python 3.11+ | Application server runtime |
| Browser | Chrome 120+, Edge 120+, Firefox 118+ | For POS terminals and admin dashboards |
| Network | 100 Mbps minimum | Required for online sync and cloud backup |
| Security | HTTPS/TLS 1.3, AES-256 encryption | Data security and compliance |
| Backup | Daily automated backup | Cloud backup via Google Drive |

---

## 9.0 System Setup & Deployment

### Prerequisites

- Hardware, software, and network requirements

### Installation Steps

1. Server setup
2. Database configuration
3. Backend/frontend deployment
4. POS terminal configuration
5. Cloud integration
6. Testing and verification

### Maintenance

- Monitoring, updates, backups, and alerting

### Notes

- Multiple POS terminals supported; offline POS sync required

---

## 10.0 Master Data Setup

### Purpose

Initialize core reference data for proper system operation.

### Entities & Required Fields

| Entity | Description | Required Fields | Notes |
|---|---|---|---|
| Menu Items | Food & beverage | Item ID, Name, Category, Price, Tax %, Stock Unit | Optional: Image, Allergens |
| Item Categories | Menu organization | Category ID, Name, Description | Example: Beverages, Appetizers |
| Tables | Dine-in tables | Table ID, Name/Number, Capacity, Status | Status: Free, Occupied, Reserved |
| Suppliers | Vendors | Supplier ID, Name, Contact, Address, Payment Terms | For inventory purchases |
| Employees | Staff | Employee ID, Name, Role, Department, Fingerprint Template, Salary | Role-based access |
| Roles & Permissions | Access control | Role ID, Name, Permissions List | Example: Manager, Cashier |
| Tax Rates | Sales tax | Tax ID, Name, Rate % | Applied in POS billing |
| Payment Methods | POS payment options | Payment ID, Name, Type | Cash, Card, Wallet |
| Shifts | HRMS shifts | Shift ID, Name, Start/End Time | Payroll calculation |
| Table Zones | Optional | Zone ID, Name, Description | Useful for large restaurants |

### Workflow

1. System Initialization → Import CSV/Excel or manual entry
2. Setup menu categories → add items → assign initial stock
3. Configure tables & zones
4. Register employees → assign roles → enroll fingerprints
5. Add suppliers → map items to suppliers
6. Configure payment methods & tax rates
7. Define shifts

### Validation & Backup

- Unique IDs, mandatory fields filled, stock ≥ 0, valid tax rates, encrypted fingerprints
- Included in daily Google Drive backups
- Master data must be completed before system goes live

---

*Document Version: 1.0 | Last Updated: 13 November 2025*
