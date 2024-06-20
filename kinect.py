from pykinect2 import PyKinectV2
from pykinect2.PyKinectV2 import *
from pykinect2 import PyKinectRuntime
import numpy as np
import cv2



kinect = PyKinectRuntime.PyKinectRuntime(PyKinectV2.FrameSourceTypes_Depth)

nothing = lambda x: None

# Create windows
cv2.namedWindow('KINECT Video Stream', cv2.WINDOW_NORMAL)
cv2.resizeWindow('KINECT Video Stream', 512, 424)




depth_min = 300
depth_max = 2300


while True:
    if kinect.has_new_depth_frame():
        frame = kinect.get_last_depth_frame()
        frame = np.reshape(frame, (424, 512))
        # flip frame
        frame = cv2.flip(frame, 0)

        min_depth = depth_min
        max_depth = depth_max

        frame = np.where((frame > min_depth) & (frame < max_depth), 255, 0).astype(np.uint8) 


        cv2.imshow('KINECT Video Stream', frame)

    key = cv2.waitKey(1)
    if key == 27: break