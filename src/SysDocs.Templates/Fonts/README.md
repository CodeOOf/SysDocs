# Fonts Directory

This directory contains embedded fonts for deterministic PDF rendering across all platforms.

## Required Fonts

To ensure deterministic output, SysDocs embeds the following open-source fonts:

### Sans-Serif (Primary)
- **Liberation Sans** - Metrically compatible with Arial
  - Regular, Bold, Italic, Bold Italic

### Serif
- **Liberation Serif** - Metrically compatible with Times New Roman
  - Regular, Bold, Italic, Bold Italic

### Monospace
- **Liberation Mono** - Metrically compatible with Courier New
  - Regular, Bold, Italic, Bold Italic

## Installation

### Option 1: Download from Liberation Fonts

1. Visit: https://github.com/liberationfonts/liberation-fonts/releases
2. Download latest release (e.g., `liberation-fonts-ttf-2.1.5.tar.gz`)
3. Extract the TTF files to this directory:
   ```
   Fonts/
   ├── LiberationSans-Regular.ttf
   ├── LiberationSans-Bold.ttf
   ├── LiberationSans-Italic.ttf
   ├── LiberationSans-BoldItalic.ttf
   ├── LiberationSerif-Regular.ttf
   ├── LiberationSerif-Bold.ttf
   ├── LiberationSerif-Italic.ttf
   ├── LiberationSerif-BoldItalic.ttf
   ├── LiberationMono-Regular.ttf
   ├── LiberationMono-Bold.ttf
   ├── LiberationMono-Italic.ttf
   └── LiberationMono-BoldItalic.ttf
   ```

### Option 2: Install via Package Manager

**Debian/Ubuntu:**
```bash
sudo apt-get install fonts-liberation
cp /usr/share/fonts/truetype/liberation/*.ttf ./
```

**Fedora/RHEL:**
```bash
sudo dnf install liberation-fonts
cp /usr/share/fonts/liberation/*.ttf ./
```

**macOS (Homebrew):**
```bash
brew install --cask font-liberation
cp /Library/Fonts/Liberation*.ttf ./
```

**Windows:**
Download from the GitHub releases link above.

## Why Embedded Fonts?

Embedding fonts ensures:
1. **Determinism** - Same output on all systems regardless of installed fonts
2. **Portability** - PDFs render identically everywhere
3. **Compliance** - Meets tool qualification requirements
4. **Legal** - Liberation fonts use SIL Open Font License

## License

Liberation Fonts are licensed under the SIL Open Font License 1.1:
https://github.com/liberationfonts/liberation-fonts/blob/main/LICENSE

This is compatible with our MIT license and suitable for embedding.

## Note

Font files (*.ttf) are **not included** in the repository to keep it lightweight.
They will be automatically included in:
- Docker images (via Dockerfile)
- Nix builds (via flake.nix)
- Release packages

For development, download them using one of the methods above.

