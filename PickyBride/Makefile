SEP="========================================================"
GQLGEN ?= github.com/99designs/gqlgen

################################################################################################################

.PHONY: create-empty-db
create-empty-db:
	$(call _info, $(SEP))
	$(call _info,"Creating empty db")
	$(call _info, $(SEP))
	docker run -d --name picky_bride_db -p 6379:6379 -e REDIS_PASSWORD=123123 redis:7-alpine
################################################################################################################