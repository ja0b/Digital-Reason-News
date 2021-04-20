# Deployments

## **Dev & Staging** - ongoing development and tests

The pipeline picks builds from the standard **env-dev** and **env-staging** branches. To deploy push the desired branch to selected environment branch and await results on the destination server instance.


## **UAT Release** - full release supposed to land on PROD environment (e.g. _2.9.11_)

Follow the general standards with GitFlow and changelog generated pipeline:

1. Create a **release branch** for the release number (e.g. _release/2.9.11_) and push it to the origin.
2. Perform the adjustments on the branch if needed e.g. last fixes related with the release.
3. Add an **annotated tag** with the release number (e.g. _2.9.11_) on the release branch and push tag to the origin.
4. The above step should trigger the **automation on Github** this process will regenerate changelog, move the tag to the master branch, syncronise the develop and master branches and finally it will also remove the release branch.
5. There is an automated trigger listening on the tag that continues the deployment process for the UAT pipeline on DevOps, this will generate the release that is deployed to the UAT environment.
6. After approved work on UAT environment, Approve (manually) the deployment for PROD environment and let the pipeline continue deployment to the preproduction slot of the PROD environment.
7. After approved work on preproduction slot, **Approve (manually) the deployment** for PROD environment and the deployment is done Magic.