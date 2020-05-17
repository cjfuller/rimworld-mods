# rimworld-mods

A monorepo for all my rimworld mods.

## Installation

For local development you'll need the `fake` build tool installed
(https://fake.build/).

Meatless is available in the [steam
workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2100272576).

None of the other mods are (yet) available on steam or anything, so they need
to be installed manually. If you're running the steam version of the game on
mac, then you can run `fake build target install` to install to the
appropriate place locally. If you're on a different platform, you'll first
need to modify `targetDir` in `build.fsx` to point to the appropriate
directory for mods on your system.

## List of mods

### CarpetColors

Add some additional colors of fine carpet. Cosmetic only.

### Meatless

Adds the possibility of creating fine and lavish meals without the use of
meat. Fine meals can be made with oyster mushrooms, which are a
light-insensitive crop growable in soil or in a mushroom bucket (similar to a
hydroponics basin for plants, but requires refueling with a small amount of
organic matter instead of power). Lavish meals can be made with meat
substitute, a product produced from a variety of vegetable matter at a
special production station. Very expensive to create.

### PainEmitter

Adds a specialized defensive turret that inflicts very painful injuries
without causing a whole lot of health damage. Useful for disabling humans for
capture. Totally unbalanced, but fun when playing out certain story scenarios.

### PortableGenerator

Patches the excellent advanced power plus mod so that the smallest nuclear
generator can be moved / uninstalled.
