# SysML v2 Test Project

This example project demonstrates SysML v2 integration capabilities, including:

- **Requirements**: Stakeholder and System requirements in SysML v2 format
- **Use Cases**: System use cases with actors and scenarios
- **Block Diagrams**: System architecture and detailed design
- **Sequence Diagrams**: Behavioral specifications and interactions

## Project Structure

```
sysml-v2-project/
├── requirements/
│   ├── stakeholder_requirements.sysml
│   ├── system_requirements.sysml
│   └── requirements_with_traces.sysml
├── use-cases/
│   ├── stakeholder_use_cases.sysml
│   └── system_use_cases.sysml
├── architecture/
│   ├── system_architecture_blocks.sysml
│   ├── system_behavior_sequences.sysml
│   ├── system_with_relationships.sysml
│   └── traced_architecture.sysml
├── design/
│   ├── detailed_design_blocks.sysml
│   ├── interaction_specifications.sysml
│   ├── timed_interactions.sysml
│   └── traced_behavior.sysml
└── format-variants/
    ├── requirements.sysml
    ├── requirements.kerml
    └── requirements.json
```

## Test Coverage

This project supports testing of:

- **FR-20**: Import SysML v2 Requirements → Stakeholder/System Requirements
- **FR-21**: Import SysML v2 Use Cases → Stakeholder/System Requirements
- **FR-22**: Import SysML v2 Block Diagrams → System Architecture/Detailed Design
- **FR-23**: Import SysML v2 Sequence Diagrams → System Architecture/Detailed Design
- **FR-24**: Preserve traceability links
- **FR-25**: Support multiple SysML v2 file formats (.sysml, .kerml, .json)

## SysML v2 Standard

This project uses the [SysML v2 standard](https://www.omgsysml.org/) as defined by the OMG.

## Expected Results

Expected PDF outputs are located in `examples/expected-results/sysml/`.
