#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

print_case() {
  local name="$1"
  local query="$2"
  local expected="$3"

  printf "\n"
  printf "\033[36m=== %s ===\033[0m\n" "$name"
  printf "Query:    %s\n" "$query"
  printf "Expected: %s\n" "$expected"

  local tmp_file="$SCRIPT_DIR/_demo_input.txt"
  printf "%s" "$query" > "$tmp_file"

  if dotnet run -- "$tmp_file"; then
    :
  fi

  rm -f "$tmp_file"
}

print_case "Case 1" 'status = "open" and owner = "sam"' "true"
print_case "Case 2" 'priority >= 3 or score > 90' "true"
print_case "Case 3" 'not archived and (priority >= 3 or owner = "max")' "false"
