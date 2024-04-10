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


depth_min = 200
depth_max = 2100
# # Create trackbars for min_depth and max_depth
# cv2.createTrackbar('Min Depth', 'KINECT Video Stream', depth_min, 5000, nothing)
# cv2.createTrackbar('Max Depth', 'KINECT Video Stream', depth_max, 5000, nothing)



while True:
    # --- Getting frames and drawing
    if kinect.has_new_depth_frame():
        frame = kinect.get_last_depth_frame()
        frame = np.reshape(frame, (424, 512))

        # Get the current position of the trackbars
        # min_depth = cv2.getTrackbarPos('Min Depth', 'KINECT Video Stream')
        # max_depth = cv2.getTrackbarPos('Max Depth', 'KINECT Video Stream')

        min_depth = depth_min
        max_depth = depth_max

        frame = np.where((frame > min_depth) & (frame < max_depth), 255, 0).astype(np.uint8) 

        frame = cv2.cvtColor(frame, cv2.COLOR_GRAY2RGB)
        # frame = 255 - frame




        cv2.imshow('KINECT Video Stream', frame)

    key = cv2.waitKey(1)
    if key == 27: break