#include "effects.h"
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

EffectsDatabase* effects_db_create(void) {
	EffectsDatabase* db = (EffectsDatabase*)malloc(sizeof(EffectsDatabase));
	if (!db) return NULL;

	db->count = 0;
	return db;
}

void effects_db_destroy(EffectsDatabase* db) {
	if (db) {
		free(db);
	}
}

int effects_db_load_from_json(EffectsDatabase* db, const char* filepath) {
	if (!db || !filepath) return -1;

	FILE* file = fopen(filepath, "r");
	if (!file) {
		return -1;
	}

	char buffer[8192];
	size_t read = fread(buffer, 1, sizeof(buffer) - 1, file);
	fclose(file);

	if (read == 0) {
		return -1;
	}
	buffer[read] = '\0';

	db->count = 0;

	const char* ptr = buffer;
	while ((ptr = strchr(ptr, '{')) != NULL) {
		if (db->count >= MAX_EFFECT_TYPES) break;

		const char* name_start = strstr(ptr, "\"name\"");
		const char* duration_start = strstr(ptr, "\"duration\"");
		const char* next_brace = strchr(ptr + 1, '}');

		if (!name_start || !duration_start || !next_brace || name_start > next_brace) {
			ptr++;
			continue;
		}

		name_start = strchr(name_start, ':');
		if (!name_start) {
			ptr++;
			continue;
		}
		name_start = strchr(name_start, '"');
		if (!name_start) {
			ptr++;
			continue;
		}
		name_start++;

		const char* name_end = strchr(name_start, '"');
		if (!name_end || name_end > next_brace) {
			ptr++;
			continue;
		}

		int name_len = (int)(name_end - name_start);
		if (name_len > MAX_EFFECT_NAME - 1) {
			name_len = MAX_EFFECT_NAME - 1;
		}
		strncpy(db->effects[db->count].name, name_start, name_len);
		db->effects[db->count].name[name_len] = '\0';

		duration_start = strchr(duration_start, ':');
		if (duration_start) {
			db->effects[db->count].base_duration = atoi(duration_start + 1);
		} else {
			db->effects[db->count].base_duration = 1;
		}

		db->count++;
		ptr = next_brace + 1;
	}

	return db->count;
}

int effects_db_find_effect(EffectsDatabase* db, const char* name) {
	if (!db || !name) return -1;

	for (int i = 0; i < db->count; i++) {
		if (strcmp(db->effects[i].name, name) == 0) {
			return i;
		}
	}
	return -1;
}

const EffectDefinition* effects_db_get_effect(EffectsDatabase* db, int index) {
	if (!db || index < 0 || index >= db->count) {
		return NULL;
	}
	return &db->effects[index];
}
