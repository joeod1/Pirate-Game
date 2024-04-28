EXTERNAL buy(type)
EXTERNAL getPrice(type)
EXTERNAL getType(type)
EXTERNAL getName()

->ArmoryStand

== function buy(type) ==
~ return 0

== function getName() ==
~ return "Mike"

== function getNotoriety() ==
~ return 3

== DefaultConversation ==
-> intro
= intro
- {~What's up?|Can I help you with something?}
+ What's your name?
-- My name is {getName()}
++ Cool. -> END
++ Did I ask?
--- Yeah? What?
+++ Oh yeah. Sorry. -> END
+++ No, I didn't.
---- Goodbye. -> END
+ How does your port feel about piracy?
~temp notor = getNotoriety()
{
    - notor == 0:
        It's really not too bad.
    - notor == 1:
        There have been a few incidents, but it doesn't bother me much.
    - notor == 2:
        My family business is in real trouble due to all the piracy.
    - notor == 3:
        I recently lost my best friend to a pirate. It's hard to imagine the future.
}
++ Hopefully that changes soon. -> END


== function getPrice(type) ==
~ return 5 

== function getType(type) ==
~ return 0

== FoodStand ==
-> intro

= intro
- Want any grub?
 + \ {~I sure do.| I'd love some.| Absolutely.} 
    -> purchase
 + \ {&No, thank you|I'm good|Nope|I'll pass|Not from here} -> END
 + \ {~What's your name?|What do you do around here?}
 -- I'm {getName()}, the food merchant. I sell food. You can buy food, if you'd like.
 ++ I would like to buy food, actually. -> purchase
 ++ Anything to support a local business. -> purchase
 ++ What a terrible name. Goodbye. -> END

= purchase
~ temp orangePrice = getPrice("orange")
~ temp foodType = getType("food")
~ temp foodPrice = getPrice("food")
- {~We have|I'm selling|I see you're checking out my|View our reasonably priced} <>
{
    - foodType == 0: 
        {~oranges and muck|muck and oranges}. {~Each unit of muck will cost you|One healthy heaping of muck goes for|My specialty product, muck, can be yours for just } {foodPrice} gold
    - foodType == 1:
        {~oranges and beef|beef and oranges|beef jerky and oranges|oranges and cured beef}
    - foodType == 2:
        {~oranges and sweets.|candy and tangy.|packaged sugar and oranges.} {~I'm willing to part with some candy for|Each helping of {~sweets|candy} will run you} {foodPrice} gold
} <>{~, and oranges for|. Buy an orange for|. Get a ball of citrus for|. If you're worried about scurvy, I recommend getting an orange for} {orangePrice}{~ gold|}.
+ {foodType == 0} What's in the muck? 
    -> mucksplanation
+ \ {~Oranges, please.|I'd like to purchase some oranges.|Oranges, please.} 
    ~ buy("orange")
    -> DONE
+ {foodType == 0}\ {~A few heapings of muck, please.|Some muck, please.|MUCK!} 
    ~ buy("food")
    -> DONE
+ {foodType == 1}\ {~Beef...|Beef, please.} 
    ~ buy("food") 
    -> DONE
+ {foodType == 2}\ {~Candy, please.|I'm in the market for candy.}
    ~ buy("food")
    -> DONE
+ \ {~Nevermind.|I changed my mind.|I forgot my purse.} -> DONE


= mucksplanation
- {~Why do you care?|What's it to you?|Don't ask questions like that.} {~Everyone loves my muck.|My muck has won awards.|It's historically cured any ailment.} {~I've never heard a single complaint.|There's a reason why I'm the only food stand here.}
+ My brother died to a bad batch of muck.
-- I guess you could say it was too muck for him to handle.
    ->mucktrauma
+ Have you heard of a certain French wagon wheel manufacturer?
-- You're no restauraunt reviewer. They don't announce themselves. 
++ You got me. I'm just your average muck enthusiast.
--- Alright. Order something, then.
+++ What are the offerings, again?
    ->purchase
+++ Do I get a discount?
---- {~Goodbye.|Funny guy.} -> END
+++ I'm a pirate. I will steal your muck shipments.
---- :/ -> END
++ I will keep this in mind when assessing whether to give your establishment a Pentagram.
--- I will keep this in mind when I next tell you the muck prices. -> END
+ Nevermind. Sorry for asking.
    ->purchase

= mucktrauma
+ It was the coldest day. Mountains of silence stood between myself and the others he left behind.
-- \ Don't test your luck if you can't handle the muck! -> mucktrauma
+ Grave robbers broke the soil on the same day he was buried. I've heard rumors about the secret ingredient in muck.
-- \ Yep, you got me! -> mucktrauma
+ I'd love some muck, actually. 
    -> purchase


== ArmoryStand ==
->intro
= intro
- {~Need any armaments?|Want some defenses against those pesky pirates?}
+ \ {~That's exactly what I need!|Yep|Sure do|If my wallet says so} -> purchase
+ No. -> END
+ Name?
-- I'm {getName()}, the local arms dealer. I ensure every trade ship has the capabilities to defend itself.
++ Can I view your selection? -> purchase
++ I'm the reason you're in business.
--- Thanks, friend! -> END

=purchase
~temp cballPrice = getPrice("cannonball")
~temp cannonPrice = getPrice("cannon")
- {~Cannon balls: {cballPrice} gold. Two cannons: {cannonPrice}.|You'll need, uhh, {cballPrice} gold for each cannon ball.. and, uh, {cannonPrice} for one set of cannons.}
+ \ {~I'd like some cannon balls.|Just the ammunition, please.} 
    ~ buy("cannonball")
    -> END
+ \ {~Cannons, please|I'd love a fresh new pair of cannons!}
    ~ buy ("cannon")
    -> END
+ \ {~On second thought, nevermind.|I'll find my weapons elsewhere.} -> END

