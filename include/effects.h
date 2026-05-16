#ifndef EFFECTS_H
#define EFFECTS_H

#define MAX_EFFECT_TYPES 100
#define MAX_EFFECT_NAME 256

typedef struct {
	char name[MAX_EFFECT_NAME];
	int base_duration;
} EffectDefinition;

typedef struct {
	EffectDefinition effects[MAX_EFFECT_TYPES];
	int count;
} EffectsDatabase;

EffectsDatabase* effects_db_create(void);
void effects_db_destroy(EffectsDatabase* db);

int effects_db_load_from_json(EffectsDatabase* db, const char* filepath);

int effects_db_find_effect(EffectsDatabase* db, const char* name);
const EffectDefinition* effects_db_get_effect(EffectsDatabase* db, int index);

#endif
