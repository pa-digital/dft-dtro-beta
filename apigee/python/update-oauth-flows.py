# This script reads the openapi spec and updates all the flows in the default.xml file in the proxies directory with the new oauth polices
   
import yaml
import sys
import xml.etree.ElementTree as ET

# Load the YAML file specified as the first command-line argument
with open(sys.argv[1], "r") as file:
    apds = yaml.safe_load(file)

proxy_xml_path = sys.argv[2]
tree = ET.parse(proxy_xml_path)
root = tree.getroot()

# Get oauth scopes
scopes = list(apds["components"]["securitySchemes"]["oAuth"]["flows"]["clientCredentials"]["scopes"].keys())

# Extract the paths section of the YAML file
paths = apds["paths"]

# Iterate through each path and its methods
for path, methods in paths.items():
    for method_name, method_details in methods.items():

        # Check if the method name is 'parameters'
        if method_name == "parameters":
            continue

        # Check if 'security' key exists in the method details
        if "security" in method_details:
            scopes = sorted(method_details['security'][0]['oAuth'])
            filename = f"OAuthV2-Verify-Access-Token-{'-'.join([scope.title() for scope in scopes])}-Scopes"

            flow = root.find(f".//Flow[@name='{method_details['operationId']}']")
            for step in flow.findall(".//Request/Step/Name"):
                if step.text == "OAuth-v20-1":
                    step.text = filename

tree.write(proxy_xml_path, encoding='utf-8', xml_declaration=True)

print("Update OAuth Flows function complete")
