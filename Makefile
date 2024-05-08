git-credential-config: create-credential-exclusions-file-if-not-exists bind-pre-commit

bind-pre-commit:
	chmod -R +x .githooks
	git config --local core.hooksPath .githooks/

create-credential-exclusions-file-if-not-exists:
ifeq (,$(wildcard credential-scan-exclusions.txt))
	echo "# File contents should include line separated regex filepaths (analagous to a gitignore file)" > credential-scan-exclusions.txt
endif

credential-scan-git-verified:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --only-verified --fail

credential-scan-git-unverified:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --fail --exclude-paths /opt/credential-scan-exclusions.txt

credential-scan-git-verified-no-update-no-log:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --only-verified --no-update --fail > /dev/null

credential-scan-git-unverified-no-update-no-log:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --no-update --fail > /dev/null

build:
	dotnet restore src/DfT.DTRO && dotnet build src/DfT.DTRO

test:
	dotnet test

build-test:
	build test

docker-build-push:
	gcloud builds submit

# docker-push:
# 	docker tag dtro-publish-api $(REGISTRY_URL)/dtro-prototype-publish:latest
# 	docker push $(REGISTRY_URL)/dtro-prototype-publish:latest

# 	docker tag dtro-search-api $(REGISTRY_URL)/dtro-prototype-search:latest
# 	docker push $(REGISTRY_URL)/dtro-prototype-search:latest

# 	docker tag dtro-postgres-migrations-job $(REGISTRY_URL)/dtro-prototype-postgres-migrations-job:latest
# 	docker push $(REGISTRY_URL)/dtro-prototype-postgres-migrations-job:latest

init:
	cd terraform && make init

deploy:
	cd terraform && make apply-auto-approve

docker-run:
	docker run -e ASPNETCORE_ENVIRONMENT=development --rm --name dtro-publish-api -p 8000:8080 dtro-publish-api &
	docker run -e ASPNETCORE_ENVIRONMENT=development --rm --name dtro-search-api -p 8001:8080 dtro-search-api

plan:
	cd terraform && make plan

up: docker-build docker-run

down:
	docker stop dtro-publish-api
	docker stop dtro-search-api
