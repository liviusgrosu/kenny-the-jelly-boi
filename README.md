<p float="left">
    <img src="https://raw.githubusercontent.com/liviusgrosu/kenny-the-jelly-boi/master/Pictures/Banner.png">
</p>

# Inverse Kinematics Spider

##### Table of Contents  
* [Summary](#summary)
* [Research](#research)
* [Controls](#controls)
## Summary:


Kenny the Jelly Boi is a top-down exploration game where you take control of a cute slime named Kenny. The game is quiet simple:
* Explore the area
* Collect gems
* Avoid baddies
* Get to the end

Jump through a world brimming with treasure and mysterious but watch out for the monsters that stalk the level. Explore secret paths
with extra goodies to reach a top score. Move blocks to create paths and collect keys to explore new parts of the level. Push those same blocks on buttons to bring up ancient bridges lying dormant in the waters.  

<p float="left">
    <img src="https://raw.githubusercontent.com/liviusgrosu/kenny-the-jelly-boi/master/Pictures/TileScreenshot.png">
</p>

## Research:

This game was a side project mainly to explore the ideas of UX/UI design in the mobile space. There are 2 discoveries that I found most crucial in regards on developing any future mobile releases.

1) Orientation
* This may sound trivial and obvious however this area took me a bit of time to understand why UI in phones are designed a certain way. Originally I had my orientation set to landscape thinking that this would provide the most comfortable feeling and would also allow for more screen real estate. However after further testing, I found this design decision to not match my game design. 

* There's nothing inherintly wrong about this orientation but you must choose wisely depending on what kind of game you're trying to make. If we take for example, a game like Call of Duty: Mobile, there are a multitude of controls and options at the users disposal thus its inherintly clear that the user should use two hands hence using landscape (Plus taking into consideration of praticallity considering it is a FPS). However, if a game requires only one hand to play and the potrait aspect ratio is not a comprimise, then we might want to consider that over landscape. 

* Having your game in portrait accomplishes two things: 
    1) It doesn't ask the user to rotate their phone just to interact with your game. 
    2) The user only needs to use one hand

  These two points sound trivial but lowering the barrier of entry for your game helps in the long run, considering you don't have the tactical feedback of physical controls.

2) Button Placement

* Considering that the developer has chosen to use portrait as their orientation, they might want to ask themselves where they might place buttons for their game controls. Taking a look at this research article by Steven Hoober (https://www.uxmatters.com/mt/archives/2013/02/how-do-users-really-hold-mobile-devices.php), we learn that a sample size of 49% phone users hold their device with one hand. Targeting the largest demographic, we can safely assume that the controls should be placed within tapping distance of the thumb.

* As for what side of the screen those buttons should be placed will largely depend on the size of the users device. In anticipation of this, I have made the buttons anchor to the bottom of the screen. The buttons will retain the same width but not overlay a majority of the screen.

## Controls:

| Actions            | Key                                                               |
| ------------------ | ----------------------------------------------------------------- |
| Up Button          | Move Forwards                                                     |
| Down Button        | Move Backwards                                                    |
| Left Button        | Move Leftwards                                                    |
| Right Button       | Move Rightwards                                                   |
| Top Right Button   | Turn Counter Clockwise                                            |
| Top Left Button    | Turn Clockwise                                                    |
