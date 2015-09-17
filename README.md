# NeverEatLunch
Playing with D.I., mocking frameworks

Design decisions : Input with extra whitespace around integer arguments is acceptable, but not between the first string (mealtime) and the first comma.
i.e. "morning, 1, 2 ,  3" is acceptable, "morning  , 1, 2" is not acceptable, without defining a "morning  " menu.
