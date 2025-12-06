# SysDocs Templates

This directory contains SE artifact templates for INCOSE and V-Model compliance.

## Template Structure

```
Templates/
├── Default/              # Basic document template
│   ├── template.json     # Template configuration
│   └── styles.json       # Style definitions
├── INCOSE/              # INCOSE SE Handbook templates
│   ├── StakeholderRequirements.json
│   ├── SystemRequirements.json
│   └── Architecture.json
└── VModel/              # V-Model templates
    ├── Requirements.json
    ├── Design.json
    └── Verification.json
```

## Template Format

Templates are defined in JSON format:

```json
{
  "name": "Default",
  "version": "1.0.0",
  "description": "Basic document template",
  "metadata": {
    "author": "SysDocs",
    "created": "2025-12-06"
  },
  "styles": {
    "font": {
      "family": "Liberation Sans",
      "size": 11
    },
    "heading1": {
      "size": 18,
      "bold": true,
      "color": "#000000"
    },
    "heading2": {
      "size": 16,
      "bold": true
    }
  },
  "layout": {
    "pageSize": "A4",
    "margins": {
      "top": 25,
      "bottom": 25,
      "left": 25,
      "right": 25
    },
    "header": {
      "enabled": true,
      "height": 15
    },
    "footer": {
      "enabled": true,
      "height": 15
    }
  }
}
```

## Creating Custom Templates

1. Copy an existing template directory
2. Modify the JSON configuration
3. Test with sample documents
4. Submit PR with new template

See [CONTRIBUTING.md](../../CONTRIBUTING.md) for details.

