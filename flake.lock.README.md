# Nix Flake Lock File

This file pins exact versions of all Nix dependencies for reproducibility.

**DO NOT EDIT MANUALLY**

To update dependencies:
```bash
nix flake update
```

To lock to specific commit:
```bash
nix flake lock --override-input nixpkgs github:NixOS/nixpkgs/COMMIT_HASH
```

Always commit `flake.lock` after updates to ensure everyone uses the same dependency versions.

