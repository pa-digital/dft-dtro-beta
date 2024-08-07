# The script takes the OAuth xml file as a template and creates a policy for all combiniations of scope
# It also updates the top level xml file with the new oauth polices

import xml.etree.ElementTree as ET
import sys
from itertools import combinations
from pathlib import Path

scopes = sys.argv[2].split(",")

# Generate all combinations of scopes
all_scope_combinations = []
for r in range(1, len(scopes) + 1):
    combinations_r = list(combinations(scopes, r))
    all_scope_combinations.extend(combinations_r)

# Create 'oauth' folder if it doesn't exist for storing updated xml files
oauth_folder = Path().resolve() / sys.argv[3]
oauth_folder.mkdir(parents=True, exist_ok=True)

# Read server.xml to be updated with new policies
server_xml_path = sys.argv[4]
server_tree = ET.parse(server_xml_path)
server_root = server_tree.getroot()

# Find the Policies element
policies_element = server_root.find('.//Policies')

# Remove the OAuth-v20-1 policy
for policy in policies_element.findall('Policy'):
    if policy.text == 'OAuth-v20-1':
        policies_element.remove(policy)


# Loop through the scopes
for scopes in all_scope_combinations:

    # Parse the XML file
    tree = ET.parse(sys.argv[1])
    root = tree.getroot()

    root.find("Scope").text = " ".join(scopes)

    # Construct the filename based on scope combination
    filename = f"OAuthV2-Verify-Access-Token-{'-'.join([scope.title() for scope in sorted(scopes)])}-Scopes"
    filepath = oauth_folder / filename

    # Update display name
    root.find('DisplayName').text = filename

    # Update name tag
    root.set("name", filename)

    # Write file with updated xml
    tree.write(str(filepath) + ".xml", encoding='UTF-8', xml_declaration=True)

    # Add the new policy to server.xml
    policy_element = ET.Element('Policy')
    policy_element.text = filename
    policies_element.append(policy_element)

# Save the updated XML file
server_tree.write(server_xml_path, encoding='utf-8', xml_declaration=True)

print("Generate OAuth Flows function complete")
