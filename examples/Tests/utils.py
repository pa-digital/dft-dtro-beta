import json

GREEN = "\033[92m"
RED = "\033[91m"
RESET = "\033[0m"

def get_json_schema():
    with open("../Schemas/schema-3.3.0-revised.json", "r") as f:
        schema = json.load(f)

    return schema

def get_root_json():
    with open("root_json.json", "r") as f:
        root_example_file = json.load(f)

    return root_example_file

def print_success():
    print(f"{GREEN}●{RESET}", end=" ")

def print_failure():
    print(f"{RED}●{RESET}", end=" ")
