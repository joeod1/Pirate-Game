{"inkVersion":21,"root":[[["done",{"#f":5,"#n":"g-0"}],null],"done",{"buy":[{"temp=":"type"},"ev",0,"/ev","~ret",{"#f":1}],"FoodStand":[{"->":".^.intro"},{"intro":[[["^Want any grub?","\n",["ev",{"^->":"FoodStand.intro.0.g-0.2.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-0","flg":18},{"s":["^ ",["ev","visit",5,"%","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"ev","du",3,"==","/ev",{"->":".^.s3","c":true},"ev","du",4,"==","/ev",{"->":".^.s4","c":true},"nop",{"s0":["pop","^No, thank you",{"->":".^.^.35"},null],"s1":["pop","^I'm good",{"->":".^.^.35"},null],"s2":["pop","^Nope",{"->":".^.^.35"},null],"s3":["pop","^I'll pass",{"->":".^.^.35"},null],"s4":["pop","^Not from here",{"->":".^.^.35"},null],"#f":5}],"^ ",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.intro.0.g-0.3.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-1","flg":18},{"s":["^ ",["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^I sure do.",{"->":".^.^.23"},null],"s1":["pop","^ I'd love some.",{"->":".^.^.23"},null],"s2":["pop","^ Absolutely.",{"->":".^.^.23"},null],"#f":5}],"^ ",{"->":"$r","var":true},null]}],{"c-0":["ev",{"^->":"FoodStand.intro.0.g-0.c-0.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.2.s"},[{"#n":"$r2"}],"end","\n",{"#f":5}],"c-1":["ev",{"^->":"FoodStand.intro.0.g-0.c-1.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.3.s"},[{"#n":"$r2"}],"\n",{"->":"FoodStand.purchase"},{"#f":5}],"#f":5,"#n":"g-0"}],null],{"#f":1}],"purchase":[[[["ev","visit",4,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"ev","du",3,"==","/ev",{"->":".^.s3","c":true},"nop",{"s0":["pop","^We have",{"->":".^.^.29"},null],"s1":["pop","^I'm selling",{"->":".^.^.29"},null],"s2":["pop","^I see you're checking out my",{"->":".^.^.29"},null],"s3":["pop","^View our reasonably priced ",{"->":".^.^.29"},null],"#f":5}],"^ ","<>","\n",["ev",{"VAR?":"foodType"},0,"==","/ev",{"->":".^.b","c":true},{"b":["\n",["ev","visit",2,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"nop",{"s0":["pop","^oranges and muck",{"->":".^.^.17"},null],"s1":["pop","^muck and oranges",{"->":".^.^.17"},null],"#f":5}],"^. ",["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^Each unit of muck will cost you",{"->":".^.^.23"},null],"s1":["pop","^One healthy heaping of muck goes for",{"->":".^.^.23"},null],"s2":["pop","^My specialty product, muck, can be yours for just ",{"->":".^.^.23"},null],"#f":5}],"^ ","ev",{"VAR?":"foodPrice"},"out","/ev","^ gold",["ev","visit",4,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"ev","du",3,"==","/ev",{"->":".^.s3","c":true},"nop",{"s0":["pop","^, and oranges",{"->":".^.^.29"},null],"s1":["pop","^. Buy an orange for",{"->":".^.^.29"},null],"s2":["pop","^. Get a ball of citrus for",{"->":".^.^.29"},null],"s3":["pop","^. If you're worried about scurvy, I recommend getting an orange for",{"->":".^.^.29"},null],"#f":5}],"^ ","ev",{"VAR?":"orangePrice"},"out","/ev",["ev","visit",2,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"nop",{"s0":["pop","^ gold",{"->":".^.^.17"},null],"s1":["pop",{"->":".^.^.17"},null],"#f":5}],"^.","\n",{"->":".^.^.^.6"},null]}],["ev",{"VAR?":"foodType"},1,"==","/ev",{"->":".^.b","c":true},{"b":["\n",["ev","visit",4,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"ev","du",3,"==","/ev",{"->":".^.s3","c":true},"nop",{"s0":["pop","^oranges and beef",{"->":".^.^.29"},null],"s1":["pop","^beef and oranges",{"->":".^.^.29"},null],"s2":["pop","^beef jerky and oranges",{"->":".^.^.29"},null],"s3":["pop","^oranges and cured beef",{"->":".^.^.29"},null],"#f":5}],"\n",{"->":".^.^.^.6"},null]}],"nop","\n",["ev",{"^->":"FoodStand.purchase.0.g-0.8.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str",{"VAR?":"foodType"},0,"==","/ev",{"*":".^.^.c-0","flg":3},{"s":["^What's in the muck? ",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.purchase.0.g-0.9.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-1","flg":2},{"s":["^ ",["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^Oranges, please.",{"->":".^.^.23"},null],"s1":["pop","^I'd like to purchase some oranges.",{"->":".^.^.23"},null],"s2":["pop","^Oranges, please.",{"->":".^.^.23"},null],"#f":5}],"^ ",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.purchase.0.g-0.10.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str",{"VAR?":"foodType"},0,"==","/ev",{"*":".^.^.c-2","flg":3},{"s":["^ ",["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^A few heapings of muck, please.",{"->":".^.^.23"},null],"s1":["pop","^Some muck, please.",{"->":".^.^.23"},null],"s2":["pop","^MUCK!",{"->":".^.^.23"},null],"#f":5}],"^ ",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.purchase.0.g-0.11.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str",{"VAR?":"foodType"},1,"==","/ev",{"*":".^.^.c-3","flg":3},{"s":["^ ",["ev","visit",2,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"nop",{"s0":["pop","^Beef...",{"->":".^.^.17"},null],"s1":["pop","^Beef, please.",{"->":".^.^.17"},null],"#f":5}],"^ ",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.purchase.0.g-0.12.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-4","flg":2},{"s":["^ ",["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^Nevermind.",{"->":".^.^.23"},null],"s1":["pop","^I changed my mind.",{"->":".^.^.23"},null],"s2":["pop","^I forgot my purse.",{"->":".^.^.23"},null],"#f":5}],"^ ",{"->":"$r","var":true},null]}],{"c-0":["ev",{"^->":"FoodStand.purchase.0.g-0.c-0.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.8.s"},[{"#n":"$r2"}],"\n",{"->":"FoodStand.mucksplanation"},{"#f":5}],"c-1":["ev",{"^->":"FoodStand.purchase.0.g-0.c-1.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.9.s"},[{"#n":"$r2"}],"\n","ev",1,{"x()":"buy","exArgs":1},"pop","/ev","\n","done",{"#f":5}],"c-2":["ev",{"^->":"FoodStand.purchase.0.g-0.c-2.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.10.s"},[{"#n":"$r2"}],"\n","ev",2,{"x()":"buy","exArgs":1},"pop","/ev","\n","done",{"#f":5}],"c-3":["ev",{"^->":"FoodStand.purchase.0.g-0.c-3.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.11.s"},[{"#n":"$r2"}],"\n","ev",2,{"x()":"buy","exArgs":1},"pop","/ev","\n","done",{"#f":5}],"c-4":["ev",{"^->":"FoodStand.purchase.0.g-0.c-4.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.12.s"},[{"#n":"$r2"}],"done","\n",{"#f":5}],"#f":5,"#n":"g-0"}],null],{"#f":1}],"mucksplanation":[[[["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^Why do you care?",{"->":".^.^.23"},null],"s1":["pop","^What's it to you?",{"->":".^.^.23"},null],"s2":["pop","^Don't ask questions like that.",{"->":".^.^.23"},null],"#f":5}],"^ ",["ev","visit",3,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"nop",{"s0":["pop","^Everyone loves my muck.",{"->":".^.^.23"},null],"s1":["pop","^My muck has won awards.",{"->":".^.^.23"},null],"s2":["pop","^It's historically cured any ailment.",{"->":".^.^.23"},null],"#f":5}],"^ ",["ev","visit",2,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"nop",{"s0":["pop","^I've never heard a single complaint.",{"->":".^.^.17"},null],"s1":["pop","^There's a reason why I'm the only food stand here.",{"->":".^.^.17"},null],"#f":5}],"\n",["ev",{"^->":"FoodStand.mucksplanation.0.g-0.6.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-0","flg":2},{"s":["^My brother died to a bad batch of muck.",{"->":"$r","var":true},null]}],{"c-0":["ev",{"^->":"FoodStand.mucksplanation.0.g-0.c-0.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.6.s"},[{"#n":"$r2"}],"\n",[["^I guess you could say it was too muck for him to handle.","\n",{"->":"FoodStand.mucktrauma"},{"#f":5,"#n":"g-0"}],null],{"#f":5}],"#f":5,"#n":"g-0"}],null],{"#f":1}],"mucktrauma":[[["ev",{"^->":"FoodStand.mucktrauma.0.0.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-0","flg":2},{"s":["^It was the coldest day. Mountains of silence stood between myself and the others he left behind.",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.mucktrauma.0.1.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-1","flg":2},{"s":["^Grave robbers broke the soil on the same day he was buried. I've heard rumors about the secret ingredient in muck.",{"->":"$r","var":true},null]}],["ev",{"^->":"FoodStand.mucktrauma.0.2.$r1"},{"temp=":"$r"},"str",{"->":".^.s"},[{"#n":"$r1"}],"/str","/ev",{"*":".^.^.c-2","flg":2},{"s":["^I'd love some muck, actually. ",{"->":"$r","var":true},null]}],{"c-0":["ev",{"^->":"FoodStand.mucktrauma.0.c-0.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.0.s"},[{"#n":"$r2"}],"\n",[["^ ",["ev","visit",4,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"ev","du",2,"==","/ev",{"->":".^.s2","c":true},"ev","du",3,"==","/ev",{"->":".^.s3","c":true},"nop",{"s0":["pop","^You're a real downer.",{"->":".^.^.29"},null],"s1":["pop","^Wasn't my muck.",{"->":".^.^.29"},null],"s2":["pop","^Tough.",{"->":".^.^.29"},null],"s3":["pop","^Don't test your luck if you can't handle the muck!",{"->":".^.^.29"},null],"#f":5}],"^ ",{"->":".^.^.^.^.^"},"\n",{"#f":5,"#n":"g-0"}],null],{"#f":5}],"c-1":["ev",{"^->":"FoodStand.mucktrauma.0.c-1.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.1.s"},[{"#n":"$r2"}],"\n",[["^ ",["ev","visit",2,"seq","/ev","ev","du",0,"==","/ev",{"->":".^.s0","c":true},"ev","du",1,"==","/ev",{"->":".^.s1","c":true},"nop",{"s0":["pop","^You think I'm making muck out of dead bodies? You're delusional.",{"->":".^.^.17"},null],"s1":["pop","^Yep, you got me! I grind dead bodies into muck!",{"->":".^.^.17"},null],"#f":5}],"^ ",{"->":".^.^.^.^.^"},"\n",{"#f":5,"#n":"g-0"}],null],{"#f":5}],"c-2":["ev",{"^->":"FoodStand.mucktrauma.0.c-2.$r2"},"/ev",{"temp=":"$r"},{"->":".^.^.2.s"},[{"#n":"$r2"}],"\n",{"->":".^.^.^.^.purchase"},{"#f":5}]}],{"#f":1}],"#f":1}],"global decl":["ev",5,{"VAR=":"orangePrice"},5,{"VAR=":"foodPrice"},0,{"VAR=":"foodType"},"/ev","end",null],"#f":1}],"listDefs":{}}