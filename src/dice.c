#include "dice.h"
#include <stdlib.h>
#include <string.h>
#include <time.h>

static int initialized = 0;

static void init_rand(void) {
	if (!initialized) {
		srand((unsigned)time(NULL));
		initialized = 1;
	}
}

DiceType dice_type_from_string(const char* str) {
	if (!str) return D20;

	if (strcmp(str, "d3") == 0) return D3;
	if (strcmp(str, "d4") == 0) return D4;
	if (strcmp(str, "d6") == 0) return D6;
	if (strcmp(str, "d8") == 0) return D8;
	if (strcmp(str, "d10") == 0) return D10;
	if (strcmp(str, "d12") == 0) return D12;
	if (strcmp(str, "d20") == 0) return D20;
	if (strcmp(str, "d100") == 0) return D100;

	return D20;
}

const char* dice_type_to_string(DiceType type) {
	switch (type) {
		case D3:   return "d3";
		case D4:   return "d4";
		case D6:   return "d6";
		case D8:   return "d8";
		case D10:  return "d10";
		case D12:  return "d12";
		case D20:  return "d20";
		case D100: return "d100";
		default:   return "d20";
	}
}

int roll_single_die(DiceType type) {
	init_rand();
	return (rand() % (int)type) + 1;
}

RollResult* roll_dice(DiceType type, int count, int modifier) {
	if (count <= 0 || count > 1000) {
		return NULL;
	}

	init_rand();

	RollResult* result = (RollResult*)malloc(sizeof(RollResult));
	if (!result) return NULL;

	result->count = count;
	result->dice_type = type;
	result->num_dice = count;
	result->modifier = modifier;
	result->total = 0;

	for (int i = 0; i < count; i++) {
		result->individual_rolls[i] = roll_single_die(type);
		result->modified_rolls[i] = result->individual_rolls[i] + modifier;
		result->total += result->modified_rolls[i];
	}

	return result;
}

void roll_result_destroy(RollResult* result) {
	if (result) {
		free(result);
	}
}
