#ifndef DICE_H
#define DICE_H

typedef enum {
	D3 = 3,
	D4 = 4,
	D6 = 6,
	D8 = 8,
	D10 = 10,
	D12 = 12,
	D20 = 20,
	D100 = 100
} DiceType;

typedef struct {
	int individual_rolls[1000];
	int modified_rolls[1000];
	int count;
	int total;
	DiceType dice_type;
	int num_dice;
	int modifier;
} RollResult;

DiceType dice_type_from_string(const char* str);
const char* dice_type_to_string(DiceType type);

RollResult* roll_dice(DiceType type, int count, int modifier);
void roll_result_destroy(RollResult* result);

int roll_single_die(DiceType type);

#endif
