import colorsys
from pykinect2 import PyKinectV2
from pykinect2.PyKinectV2 import *
from pykinect2 import PyKinectRuntime
import numpy as np
import cv2
import pyvirtualcam



kinect = PyKinectRuntime.PyKinectRuntime(PyKinectV2.FrameSourceTypes_Depth)

nothing = lambda x: None

# Create windows
cv2.namedWindow('KINECT Video Stream', cv2.WINDOW_NORMAL)
# make window fixed size 512 424
cv2.resizeWindow('KINECT Video Stream', 512, 424)
# Flip the feed horizontally



depth_min = 300
depth_max = 2300
# # Create trackbars for min_depth and max_depth
# cv2.createTrackbar('Min Depth', 'KINECT Video Stream', depth_min, 5000, nothing)
# cv2.createTrackbar('Max Depth', 'KINECT Video Stream', depth_max, 5000, nothing)



while True:
    # --- Getting frames and drawing
    if kinect.has_new_depth_frame():
        frame = kinect.get_last_depth_frame()
        frame = np.reshape(frame, (424, 512))
        # flip frame
        frame = cv2.flip(frame, 0)

        # Get the current position of the trackbars
        # min_depth = cv2.getTrackbarPos('Min Depth', 'KINECT Video Stream')
        # max_depth = cv2.getTrackbarPos('Max Depth', 'KINECT Video Stream')

        min_depth = depth_min
        max_depth = depth_max

        frame = np.where((frame > min_depth) & (frame < max_depth), 255, 0).astype(np.uint8) 

        # frame = cv2.cvtColor(frame, cv2.COLOR_GRAY2RGB)
        # make every black pixel 70 33 29
        # frame[np.where((frame == [0,0,0]).all(axis = 2))] = [70, 33, 29]
        # if no kinect feed is available, show a black [70, 33, 29] screen
    # else:
    #     frame = np.zeros((424, 512, 3), np.uint8)
    #     frame[:] = [0, 0, 0]
    #     # add text to the frame "No Kinect feed available"
    #     cv2.putText(frame, 'No Kinect feed available', (10, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 255, 255), 2, cv2.LINE_AA)
    #     # flip the frame
    #     frame = cv2.flip(frame, 0)
    #     frame = cv2.flip(frame, 1)



        






        cv2.imshow('KINECT Video Stream', frame)

    key = cv2.waitKey(1)
    if key == 27: break